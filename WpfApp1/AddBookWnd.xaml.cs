using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddBookWnd.xaml
    /// </summary>
    public partial class AddBookWnd : Window
    {
        decimal cataId;
        public AddBookWnd(decimal cataId)
        {
            InitializeComponent();

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

            this.cataId = cataId;
            using (BookstoreEntities context = new BookstoreEntities())
            {
                var query = (from b in context.Book
                             select new BookData
                             {
                                 Id = b.Id,
                                 Author = b.Author,
                                 Title = b.Title,
                                 Genre = b.Genre,
                                 ReleaseYear = b.ReleaseYear,
                                 RetailPrice = b.RetailPrice
                             }).ToList();
                dataGrid.ItemsSource = query;
            }
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

        private void DataGridRow_Selected(object sender, RoutedEventArgs e)
        {
            var b = (sender as DataGridRow).DataContext as BookData;
            tbAuthor.Text = b.Author;
            tbTitle.Text = b.Title;
            tbGenre.Text = b.Genre;
            tbReleaseYear.Text = b.ReleaseYear.ToString();
            tbRetailPrice.Text = b.RetailPrice.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbAuthor.Text.Length > 0 && tbTitle.Text.Length > 0 &&
                tbGenre.Text.Length > 0 && tbReleaseYear.Text.Length > 0 &&
                tbRetailPrice.Text.Length > 0 && tbWholesalePrice.Text.Length > 0)
            {
                Book book = new Book();
                book.Author = tbAuthor.Text;
                book.Title = tbTitle.Text;
                book.Genre = tbGenre.Text;
                book.ReleaseYear = Convert.ToDecimal(tbReleaseYear.Text);
                book.RetailPrice = Convert.ToDecimal(tbRetailPrice.Text);

                using (BookstoreEntities context = new BookstoreEntities())
                {
                    var bk = (from b in context.Book
                              where b.Author == book.Author && b.Title == book.Title
                              select b).ToList();
                    if (bk.Count == 0)
                    {
                        context.Book.Add(book);
                        context.SaveChanges();
                    }
                    bk = (from b in context.Book
                          where b.Author == book.Author && b.Title == book.Title
                          select b).ToList();

                    Catalog catalog = new Catalog();
                    catalog.WholesalePrice = Convert.ToDecimal(tbWholesalePrice.Text);
                    if (tbStorageQuantity.Text.Length > 0) catalog.StorageQuantity = Convert.ToDecimal(tbStorageQuantity.Text);
                    else catalog.StorageQuantity = 0;
                    if (tbSoldQuantity.Text.Length > 0) catalog.SoldQuantity = Convert.ToDecimal(tbSoldQuantity.Text);
                    else catalog.SoldQuantity = 0;
                    catalog.Store = context.Store.Find(cataId);
                    catalog.Book = bk[0];

                    var ct = (from c in context.Catalog
                              where (c.Store.Id == catalog.Store.Id && c.Book.Id == catalog.Book.Id)
                              select c).ToList();
                    if (ct.Count == 0)
                    {
                        context.Catalog.Add(catalog);
                        context.SaveChanges();
                        MsgBoxWnd mbw = new MsgBoxWnd(true, "Книга добавлена");
                        mbw.ShowDialog();
                    }
                    else
                    {
                        MsgBoxWnd mbw = new MsgBoxWnd(false, "Такая книга уже есть в каталоге!");
                        mbw.ShowDialog();
                    }
                }
            }
            else
            {
                MsgBoxWnd mbw = new MsgBoxWnd(false, "Одно или несколько полей незаполнено!");
                mbw.ShowDialog();
            }
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
    }
}
