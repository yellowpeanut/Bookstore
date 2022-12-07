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
    /// Логика взаимодействия для MsgBoxWnd.xaml
    /// </summary>
    public partial class MsgBoxWnd : Window
    {
        public MsgBoxWnd(bool success, string text)
        {
            InitializeComponent();
            textBlock.Text = text;
            if (success)
            {
                label.Content = "Успех!";
                imageOkay.Visibility = Visibility.Visible;
            }
            else
            {
                label.Content = "Ошибка!";
                imageClose.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
    }
}
