using library.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace library
{
    public partial class MainWindow : Window
    {
        private readonly Model1 _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new Model1();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Проверка правильности пароля
            var user = _context.users.FirstOrDefault(p => p.login.Equals(loginTB.Text) && p.password.Equals(passwordB.Password));
            if (user != null)
            {
                if (user.access == 1)
                {
                    ReaderWindow rw = new ReaderWindow(user.id); rw.Show();
                    this.Close();
                }
                else if (user.access == 2)
                {
                    EmployeeWindow ew = new EmployeeWindow(); ew.Show();
                    this.Close();
                }
                else if (user.access == 3)
                {
                    AdminPanel aw = new AdminPanel(); aw.Show();
                    this.Close();
                }
            }
            else { MessageBox.Show("Неверный логин или пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning); }
            
        }
    }
}
