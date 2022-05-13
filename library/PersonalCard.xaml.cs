using library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для PersonalCard.xaml
    /// </summary>
    public partial class PersonalCard : Window
    {
        public int Rid;
        private readonly Model1 _context;
        public PersonalCard(int id)
        {
            Rid = id;
            InitializeComponent();
            _context = new Model1();
            LoadCard();
        }

        public List<books> Books { get; set; }
        public List<readers> Readers { get; set; }

        private void LoadCard()
        {
            Books = _context.books.ToList();
            var reader = _context.readers.FirstOrDefault(p => p.account_id == Rid);
            PersonalDG.ItemsSource = reader.books;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReaderWindow rw = new ReaderWindow(Rid); rw.Show();
            this.Close();
        }

        // Вернуть книгу
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (PersonalDG.SelectedItem == null)
            {
                MessageBox.Show("Ошибка");
                return;
            }

            books book = (books)PersonalDG.SelectedItem;
            var reader = _context.readers.FirstOrDefault(p => p.account_id == Rid);
            reader.books.Remove(book);
            _context.SaveChanges();
            Thread.Sleep(500);
            LoadCard();

            MessageBox.Show("ХАРОШ!");
        }
    }
}
