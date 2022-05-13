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
    /// Логика взаимодействия для GiveBooks.xaml
    /// </summary>
    public partial class GiveBooks : Window
    {
        private readonly Model1 _context;
        public GiveBooks()
        {
            InitializeComponent();
            _context = new Model1();
            LoadData();
        }
        public List<books> Books { get; set; }
        public List<readers> Readers { get; set; }
        private void LoadData()
        {
            Books = _context.books.ToList();
            Readers = _context.readers.ToList();
            BooksDG.ItemsSource = Books;
            ReadersDG.ItemsSource = Readers;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow ew = new EmployeeWindow(); ew.Show();
            this.Close();
        }

        // Жесткое добавление кники к пользователю
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ReadersDG.SelectedItem != null && BooksDG.SelectedItem != null)
            {
                var _reader = ReadersDG.SelectedItem as readers;
                var _book = BooksDG.SelectedItem as books;
                _reader.books.Add(_book);
                if (_context.SaveChanges() > 0)
                {
                    MessageBox.Show("Книга выдана", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    _context.SaveChanges();
                }
                else { MessageBox.Show("Книга уже выдана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation); }

            }
        }
    }
}
