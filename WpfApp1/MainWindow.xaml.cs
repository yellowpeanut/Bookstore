using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool expanded = false;
        decimal cataId = 0;
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            loadStoresToDGV();
        }
        private void btnHideApp_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnFullscreenApp_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized) WindowState = WindowState.Maximized;
            else WindowState = WindowState.Normal;
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void loadStoresToDGV()
        {
            using (BookstoreEntities context = new BookstoreEntities())
            {
                var query = (from s in context.Store
                             select new StoreData
                             {
                                 Id = s.Id,
                                 Address = s.Address,
                                 Name = s.Name
                             }).ToList();
                dataGrid.ItemsSource = query;
            }
            btnAddBook.IsEnabled = false;
            btnDeleteBook.IsEnabled = false;
            btnStoreOrders.IsEnabled = false;
            btnInfoBook.IsEnabled = false;
            btnStoreProfit.IsEnabled = false;
            btnFindBook.IsEnabled = false;
            btnReset.IsEnabled = false;
            btnGoBack.IsEnabled = false;
        }

        private void loadCatalogToDGV()
        {
            using (BookstoreEntities context = new BookstoreEntities())
            {
                if (!expanded)
                {
                    var catalog = (from c in context.Catalog
                                   where c.CatalogId == cataId
                                   from b in context.Book
                                   where c.BookId == b.Id
                                   select new CatalogData
                                   {
                                       Id = c.CatalogId,
                                       BookId = c.BookId,
                                       Author = b.Author,
                                       Title = b.Title,
                                       Genre = b.Genre,
                                       ReleaseYear = b.ReleaseYear,
                                       RetailPrice = b.RetailPrice
                                   }).ToList();
                    dataGrid.ItemsSource = catalog;
                }
                else
                {
                    var catalog = (from c in context.Catalog
                                   where c.CatalogId == cataId
                                   from b in context.Book
                                   where c.BookId == b.Id
                                   select new CatalogDataExpanded
                                   {
                                       Id = c.CatalogId,
                                       BookId = c.BookId,
                                       Author = b.Author,
                                       Title = b.Title,
                                       Genre = b.Genre,
                                       ReleaseYear = b.ReleaseYear,
                                       RetailPrice = b.RetailPrice,
                                       WholesalePrice = c.WholesalePrice,
                                       SoldQuantity = c.SoldQuantity,
                                       StorageQuantity = c.StorageQuantity
                                   }).ToList();
                    dataGrid.ItemsSource = catalog;
                }
                btnAddBook.IsEnabled = true;
                btnDeleteBook.IsEnabled = true;
                btnStoreOrders.IsEnabled = true;
                btnInfoBook.IsEnabled = true;
                btnStoreProfit.IsEnabled = true;
                btnFindBook.IsEnabled = true;
                btnReset.IsEnabled = true;
                btnGoBack.IsEnabled = true;
            }
        }

        private void loadOrdersToDGV()
        {
            using (BookstoreEntities context = new BookstoreEntities())
            {
                var orders = (from o in context.Order
                              where o.CatalogId == cataId
                              from b in context.Book
                              where o.BookId == b.Id
                              select new
                              {
                                  Id = o.Id,
                                  BookId = o.BookId,
                                  Author = b.Author,
                                  Title = b.Title,
                                  Genre = b.Genre,
                                  ReleaseYear = b.ReleaseYear,
                                  RetailPrice = b.RetailPrice,
                                  OrderedQuantity = o.Quantity,
                                  Date = o.Date
                              }).ToList();
                dataGrid.ItemsSource = orders;
            }
            btnAddBook.IsEnabled = false;
            btnDeleteBook.IsEnabled = false;
            btnStoreOrders.IsEnabled = false;
            btnInfoBook.IsEnabled = false;
            btnFindBook.IsEnabled = false;
            btnReset.IsEnabled = false;
        }

        public void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (BookstoreEntities context = new BookstoreEntities())
            {
                if (dataGrid.SelectedIndex != -1)
                {
                    if (dataGrid.Columns.Count == 3)
                    {
                        var s = dataGrid.SelectedItem as StoreData;

                        cataId = Convert.ToDecimal(s.Id);
                        expanded = false;
                        loadCatalogToDGV();
                    }
                    /*
                    else if (dataGridView1.ColumnCount == 7)
                    {
                        decimal bookId = Convert.ToDecimal(dataGridView1[1, e.RowIndex].Value);
                        AdditionalInfoForm aif = new AdditionalInfoForm(cataId, bookId);
                        aif.ShowDialog();
                    }
                    */
                }
            }
        }
        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            AddBookWnd abw = new AddBookWnd(cataId);
            abw.Show();
        }
        private void btnDeleteBook_Click(object sender, RoutedEventArgs e)
        {
            using (BookstoreEntities context = new BookstoreEntities())
            {
                foreach (var item in dataGrid.SelectedItems)
                {
                    decimal i = 0;
                    if (!expanded) i = (item as CatalogData).BookId;
                    else i = (item as CatalogDataExpanded).BookId;
                    var row = from c in context.Catalog
                              where c.CatalogId == cataId && c.BookId == i
                              select c;
                    context.Catalog.Remove(row.First());
                }
                context.SaveChanges();
                loadCatalogToDGV();
            }
        }

        private void btnStoreOrders_Click(object sender, RoutedEventArgs e)
        {
            loadOrdersToDGV();
        }

        private void btnInfoBook_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Columns.Count == 7)
            {
                expanded = true;
                loadCatalogToDGV();
            }
            else
            {
                expanded = false;
                loadCatalogToDGV();
            }
        }

        private void btnStoreProfit_Click(object sender, RoutedEventArgs e)
        {
            using (BookstoreEntities context = new BookstoreEntities())
            {
                var query = (from c in context.Catalog
                             where c.CatalogId == cataId
                             from b in context.Book
                             where c.BookId == b.Id
                             select new
                             {
                                 b.RetailPrice,
                                 c.WholesalePrice,
                                 c.SoldQuantity
                             });
                decimal? profit = 0;
                foreach (var q in query) profit += (q.RetailPrice - q.WholesalePrice) * q.SoldQuantity;
                MsgBoxWnd mbw = new MsgBoxWnd(true, $"Прибыль от продажи книг в этом магазине составила {profit} рублей.");
                mbw.ShowDialog();
            }
        }

        private void btnAllProfits_Click(object sender, RoutedEventArgs e)
        {
            using (BookstoreEntities context = new BookstoreEntities())
            {
                var query = (from c in context.Catalog
                             from b in context.Book
                             where c.BookId == b.Id
                             select new
                             {
                                 b.RetailPrice,
                                 c.WholesalePrice,
                                 c.SoldQuantity
                             });
                decimal? profit = 0;
                foreach (var q in query) profit += (q.RetailPrice - q.WholesalePrice) * q.SoldQuantity;
                MsgBoxWnd mbw = new MsgBoxWnd(true, $"Прибыль от продажи книг во всех магазинах составила {profit} рублей.");
                mbw.ShowDialog();
            }
        }

        private void btnFindBook_Click(object sender, RoutedEventArgs e)
        {
            string genre = tbSearch.Text.Trim().ToLower();
            using (BookstoreEntities context = new BookstoreEntities())
            {
                if (!expanded)
                {
                    var catalog = (from c in context.Catalog
                                   where c.CatalogId == cataId
                                   from b in context.Book
                                   where c.BookId == b.Id && b.Genre.ToLower().Contains(genre)
                                   select new CatalogData
                                   {
                                       Id = c.CatalogId,
                                       BookId = c.BookId,
                                       Author = b.Author,
                                       Title = b.Title,
                                       Genre = b.Genre,
                                       ReleaseYear = b.ReleaseYear,
                                       RetailPrice = b.RetailPrice
                                   }).ToList();
                    if (catalog.Count > 0) dataGrid.ItemsSource = catalog;
                    else
                    {
                        MsgBoxWnd mbw = new MsgBoxWnd(false, "Книги по заданному жанру не найдены");
                        mbw.ShowDialog();
                    }
                }
                else
                {
                    var catalog = (from c in context.Catalog
                                   where c.CatalogId == cataId
                                   from b in context.Book
                                   where c.BookId == b.Id && b.Genre.ToLower().Contains(genre)
                                   select new CatalogDataExpanded
                                   {
                                       Id = c.CatalogId,
                                       BookId = c.BookId,
                                       Author = b.Author,
                                       Title = b.Title,
                                       Genre = b.Genre,
                                       ReleaseYear = b.ReleaseYear,
                                       RetailPrice = b.RetailPrice,
                                       WholesalePrice = c.WholesalePrice,
                                       SoldQuantity = c.SoldQuantity,
                                       StorageQuantity = c.StorageQuantity
                                   }).ToList();
                    if (catalog.Count > 0) dataGrid.ItemsSource = catalog;
                    else
                    {
                        MsgBoxWnd mbw = new MsgBoxWnd(false, "Книги по заданному жанру не найдены");
                        mbw.ShowDialog();
                    }
                }
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            loadCatalogToDGV();
            tbSearch.Text = "Жанр";
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Columns.Count == 9) loadCatalogToDGV();
            else loadStoresToDGV();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
    }
}
