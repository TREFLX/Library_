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
using System.Windows.Shapes;

namespace library.xaml
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        private readonly Model1 _context;
        public int b;
        private int page = 0;
        private int MaxPage = 0;
        private int TurningPage = 5;

        public Users(int a)
        {
            b = a;
            InitializeComponent();
            _context = new Model1();
            LoadUsers();
        }

        public List<readers> Reader { get; set; }
        public List<users> User { get; set; }
        //public List<books> Books { get; set; }
        // Заполнение дата-грид
        private void LoadUsers()
        {
            Reader = _context.readers.ToList();
            User = _context.users.ToList();
            if (Reader.Count % TurningPage == 0)
            {
                MaxPage = (Reader.Count / TurningPage) - 1;
            }
            else
            {
                MaxPage = Reader.Count / TurningPage;
            }
            UsersDG.ItemsSource = Reader.OrderBy(p => p.id).Skip(page * TurningPage).Take(TurningPage);
            authorCB.ItemsSource = Reader.Select(p => p.first_name).Distinct();
            delbtn.Visibility = (b == 2) ? Visibility.Visible : Visibility.Hidden;
        }
        // Добавление
        private void AddUser(object sender, RoutedEventArgs e)
        {
            var conAddwindow = new AddUser(_context);
            if (conAddwindow.ShowDialog() == true)
            {
                LoadUsers();
                MessageBox.Show("Пользователь успешно добавлен!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Изменение
        private void EditUser(object sender, RoutedEventArgs e)
        {
            if (UsersDG.SelectedItem != null)
            {
                var _readers = UsersDG.SelectedItem as readers;
                var _user = User.Find(p=>p.id == _readers.account_id);
                var conEditWindow = new AddUser(_context,_readers,_user);

                if (conEditWindow.ShowDialog() == true)
                {
                    UsersDG.ItemsSource = _context.readers.ToList();
                    MessageBox.Show("Пользователь успешно изменен!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите пользователя!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Удаление
        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            if (UsersDG.SelectedItem != null)
            {
                
                var _readers = UsersDG.SelectedItem as readers;
                _readers.books.Clear();
                _context.readers.Remove(_context.readers.FirstOrDefault(p => p.id == _readers.id));
                _context.SaveChanges();
                var _users = UsersDG.SelectedItem as users;
                _context.users.Remove(_context.users.FirstOrDefault(p => p.id == _readers.account_id));
                _context.SaveChanges();

                LoadUsers();
                MessageBox.Show("Пользователь успешно удален!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Сначала выберите пользователя!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Возврат на форму из которой перешли
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (b == 1) {
                EmployeeWindow ew = new EmployeeWindow(); ew.Show();
                this.Close();
            }
            if (b == 2) {
                AdminPanel aw = new AdminPanel(); aw.Show();
                this.Close();
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = Reader.Where(p => p.last_name.ToLower().StartsWith(searchtxt.Text.ToLower()) || p.id.ToString().ToLower().StartsWith(searchtxt.Text.ToLower())); //p.id.ToString().ToLower().StartsWith(searchtxt.Text.ToLower())
            if (searchtxt.Text != null)
            {
                UsersDG.ItemsSource = filtered;
            }
            //filter();
        }

        private void filter()
        {
            if (authorCB != null)
            {
                UsersDG.ItemsSource = Reader.Where(p => p.first_name.Contains(authorCB.Text));
            }
        }

        private void authorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            filter();
        }

        private void UpdatePage()
        {
            writerPage.Content = $"{page + 1} Страница";
        }


        // Обнуление фильтров (сброс)
        private void RefreshBook(object sender, RoutedEventArgs e)
        {
            authorCB.SelectedItem = null;
            searchtxt.Text = "";
            LoadUsers();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (page == 0) return;
            page--;
            LoadUsers();
            UpdatePage();
            if (authorCB.SelectedItem != null || searchtxt.Text.Length != 0)
            {
                filter();
            }
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (page == MaxPage) return;
            page++;
            LoadUsers();
            UpdatePage();

            if (authorCB.SelectedItem != null || searchtxt.Text.Length != 0)
            {
                writerPage.Content = "1 Страница";
                filter();
            }
        }
    }
}
