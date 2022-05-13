using library.Data;
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
    public partial class ListBooks : Window
    {
        private readonly Model1 _context;
        private int page = 0;
        private int MaxPage = 0;
        private int TurningPage = 5;

        public ListBooks()
        {
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
            publishCB.ItemsSource = Books.Select(p => p.publish).Distinct();
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
            if (publishCB.SelectedItem != null)
            {
                BooksDG.ItemsSource = Books.Where(p => p.publish.Contains(publishCB.Text));
                //BooksDG.ItemsSource = filtered;
            }
            if (authorCB.SelectedItem != null && styleCB.SelectedItem != null)
            {
                BooksDG.ItemsSource = Books.Where(p => p.author.Contains(authorCB.Text) && p.style.Contains(styleCB.Text));
                //= filtered;
            }
            if (authorCB.SelectedItem != null && publishCB.SelectedItem != null)
            {
                BooksDG.ItemsSource = Books.Where(p => p.author.Contains(authorCB.Text) && p.publish.Contains(publishCB.Text));
                //BooksDG.ItemsSource = filtered;
            }
            if (styleCB.SelectedItem != null && publishCB.SelectedItem != null)
            {
                BooksDG.ItemsSource = Books.Where(p => p.style.Contains(styleCB.Text) && p.publish.Contains(publishCB.Text));
                //BooksDG.ItemsSource = filtered;
            }
            if (authorCB.SelectedItem != null && styleCB.SelectedItem != null && publishCB.SelectedItem != null)
            {
                BooksDG.ItemsSource = Books.Where(p => p.author.Contains(authorCB.Text) && p.style.Contains(styleCB.Text) && p.publish.Contains(publishCB.Text));
                //= filtered;
            }
            var filtered = Books.Where(p => p.title.ToLower().StartsWith(searchtxt.Text.ToLower()));
            if (searchtxt.Text.Length != 0)
            {
                BooksDG.ItemsSource = filtered;
            }
        }
        // Поиск в реальном времени
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = Books.Where(p => p.title.ToLower().StartsWith(searchtxt.Text.ToLower()));
            if (searchtxt.Text != null)
            {
                BooksDG.ItemsSource = filtered;
            }
            //filter();
        } 
        // Кнопка назад
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow ew = new EmployeeWindow(); ew.Show();
            this.Close();
        }
        // Добавление книг
        private void AddBook(object sender, RoutedEventArgs e)
        {
            var conAddwindow = new EditBook(_context);
            if (conAddwindow.ShowDialog() == true)
            {
                LoadBooks();
                MessageBox.Show("Книга успешно добавлена!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Изменение книг
        private void EditBooks(object sender, RoutedEventArgs e)
        {
            if (BooksDG.SelectedItem != null)
            {
                var conEditWindow = new EditBook(_context, BooksDG.SelectedItem as books);

                if (conEditWindow.ShowDialog() == true)
                {
                    BooksDG.ItemsSource = _context.books.ToList();
                    MessageBox.Show("Книга успешно изменена!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите книгу!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Удаление книг
        private void DeleteBook(object sender, RoutedEventArgs e)
        {
            if (BooksDG.SelectedItem != null)
            {
                var contract = BooksDG.SelectedItem as books;
                _context.books.Remove(_context.books.FirstOrDefault(p => p.id == contract.id));
                _context.SaveChanges();

                LoadBooks();
                MessageBox.Show("Книга успешно удалена!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Сначала выберите книгу!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Выборка по автору в combobox
        private void authorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            filter();
        }

        private void UpdatePage() {
            writerPage.Content = $"{page + 1} Страница";
        }


        // Обнуление фильтров (сброс)
        private void RefreshBook(object sender, RoutedEventArgs e)
        {
            authorCB.SelectedItem = null;
            styleCB.SelectedItem = null;
            publishCB.SelectedItem = null;
            LoadBooks();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (page == 0) return;
            page--;
            LoadBooks();
            UpdatePage();
            if (authorCB.SelectedItem != null || styleCB.SelectedItem != null || publishCB.SelectedItem != null || searchtxt.Text.Length != 0)
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
             
            if (authorCB.SelectedItem != null || styleCB.SelectedItem != null || publishCB.SelectedItem != null || searchtxt.Text.Length != 0)
            {
                writerPage.Content = "1 Страница";
                filter();
            }
        }
    }
}
