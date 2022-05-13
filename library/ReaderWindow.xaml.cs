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
    /// Логика взаимодействия для ReaderWindow.xaml
    /// </summary>
    public partial class ReaderWindow : Window
    {
        public int Rid;
        private readonly Model1 _context;
        private int page = 0;
        private int MaxPage = 0;
        private int TurningPage = 5;
        public ReaderWindow(int id)
        {
            Rid = id;
            InitializeComponent();
            _context = new Model1();
            LoadBooks();
        }

        public List<books> Books { get; set; }
        private void LoadBooks()
        {
            Books = _context.books.ToList();
            if (Books.Count % TurningPage == 0)
            {
                MaxPage = (Books.Count / TurningPage) - 1;
            }
            else
            {
                MaxPage = Books.Count / TurningPage;
            }
            BooksDG.ItemsSource = Books.OrderBy(p => p.id).Skip(page * TurningPage).Take(TurningPage);
            authorCB.ItemsSource = Books.Select(p => p.author).Distinct();
            styleCB.ItemsSource = Books.Select(p => p.style).Distinct();
        }

        // Открытие личной карточки с передачей id
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PersonalCard pc = new PersonalCard(Rid); pc.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(); mw.Show();
            this.Close();
        }

        private void filter()
        {
            if (authorCB.SelectedItem != null)
            {
                BooksDG.ItemsSource = Books.Where(p => p.author.Contains(authorCB.Text));
                //BooksDG.ItemsSource = filtered;
            }
            if (styleCB.SelectedItem != null)
            {
                BooksDG.ItemsSource = Books.Where(p => p.style.Contains(styleCB.Text));
                //BooksDG.ItemsSource = filtered;
            }
            if (authorCB.SelectedItem != null && styleCB.SelectedItem != null)
            {
                BooksDG.ItemsSource = Books.Where(p => p.author.Contains(authorCB.Text) && p.style.Contains(styleCB.Text));
                //BooksDG.ItemsSource = filtered;
            }
            var filtered = Books.Where(p => p.title.ToLower().StartsWith(searchtxt.Text.ToLower()));
            if (searchtxt.Text.Length != 0)
            {
                BooksDG.ItemsSource = filtered;
            }
        }

        // Поиск по названию в реальном времени
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = Books.Where(p => p.title.ToLower().StartsWith(searchtxt.Text.ToLower()));
            if (searchtxt.Text != null)
            {
                BooksDG.ItemsSource = filtered; 
            }
            //filter();
        }

        // Фильтр автора, жанра (комбо бокса)
        private void FiltredCB_LostFocus(object sender, RoutedEventArgs e)
        {
            filter();
        }

        // Обнуление фильтров(сброс)
        private void RefreshBook(object sender, RoutedEventArgs e)
        {
            authorCB.SelectedItem = null;
            styleCB.SelectedItem = null;
            LoadBooks();
        }

        private void UpdatePage()
        {
            writerPage.Content = $"{page + 1} Страница";
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (page == 0) return;
            page--;
            LoadBooks();
            UpdatePage();
            if (authorCB.SelectedItem != null || styleCB.SelectedItem != null || BooksDG.SelectedItem != null || searchtxt.Text.Length != 0)
            {
                filter();
            }
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (page == MaxPage) return;
            page++;
            LoadBooks();
            UpdatePage();

            if (authorCB.SelectedItem != null || styleCB.SelectedItem != null || BooksDG.SelectedItem != null || searchtxt.Text.Length != 0)
            {
                writerPage.Content = "1 Страница";
                filter();
            }
        }
    }
}
