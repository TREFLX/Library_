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

namespace library
{
    /// <summary>
    /// Логика взаимодействия для Employees.xaml
    /// </summary>
    public partial class Employees : Window
    {
        private readonly Model1 _context;
        private int page = 0;
        private int MaxPage = 0;
        private int TurningPage = 5;

        public Employees()
        {
            InitializeComponent();
            _context = new Model1();
            LoadEmp();
        }
        public List<employees> Emps { get; set; }
        public List<users> User { get; set; }
        private void LoadEmp()
        {
            Emps = _context.employees.ToList();
            User = _context.users.ToList();
            if (Emps.Count % TurningPage == 0)
            {
                MaxPage = (Emps.Count / TurningPage) - 1;
            }
            else
            {
                MaxPage = Emps.Count / TurningPage;
            }
            EmpDG.ItemsSource = Emps.OrderBy(p => p.id).Skip(page * TurningPage).Take(TurningPage);
        }
        // Добавление
        private void AddEmp(object sender, RoutedEventArgs e)
        {
            var conAddwindow = new AddEmps(_context);
            if (conAddwindow.ShowDialog() == true)
            {
                LoadEmp();
                MessageBox.Show("Сотрудник успешно добавлен!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Изменение
        private void EditEmp(object sender, RoutedEventArgs e)
        {
            if (EmpDG.SelectedItem != null)
            {
                var _emps = EmpDG.SelectedItem as employees;
                var _user = User.Find(p => p.id == _emps.account_id);
                var conEditWindow = new AddEmps(_context, _emps, _user); //adduser заменить

                if (conEditWindow.ShowDialog() == true)
                {
                    EmpDG.ItemsSource = _context.employees.ToList();
                    MessageBox.Show("Сотрудник успешно изменен!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите сотрудника!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Удаление
        private void DeleteEmp(object sender, RoutedEventArgs e)
        {
            if (EmpDG.SelectedItem != null)
            {
                var _emps = EmpDG.SelectedItem as employees;
                _context.employees.Remove(_context.employees.FirstOrDefault(p => p.id == _emps.id));
                _context.SaveChanges();
                var _users = EmpDG.SelectedItem as users;
                _context.users.Remove(_context.users.FirstOrDefault(p => p.id == _emps.account_id));
                _context.SaveChanges();

                page = 0;
                LoadEmp();
                UpdatePage();
                MessageBox.Show("Сотрудник успешно удален!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Сначала выберите сотрудника!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel aw = new AdminPanel(); aw.Show();
            this.Close();
        }

        private void UpdatePage()
        {
            writerPage.Content = $"{page + 1} Страница";
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (page == 0) return;
            page--;
            LoadEmp();
            UpdatePage();
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (page == MaxPage) return;
            page++;
            LoadEmp();
            UpdatePage();
        }
    }
}
