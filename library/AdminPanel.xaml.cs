using library.xaml;
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

namespace library
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(); mw.Show();
            this.Close();
        }
        private void UsersButton(object sender, RoutedEventArgs e)
        {
            Users uw = new Users(2); uw.Show();
            this.Close();
        }
        private void EmployeesButton(object sender, RoutedEventArgs e)
        {
            Employees mw = new Employees(); mw.Show();
            this.Close();
        }
    }
}
