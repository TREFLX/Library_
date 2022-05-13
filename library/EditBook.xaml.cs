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
    /// Логика взаимодействия для EditBook.xaml
    /// </summary>
    public partial class EditBook : Window
    {
        private readonly Model1 _context;
        private bool _isBookEdit = false;
        private books _books;
        public EditBook(Model1 context)
        {
            InitializeComponent();
            _context = context;
            _books = new books();
            this.Title = "Добавление книги";
        }
        public EditBook(Model1 context,books book) : this(context)
        {
            _isBookEdit = true;
            _books = book;
            txtTitle.Text = book.title;
            txtAuthor.Text = book.author;
            txtStyle.Text = book.style;
            txtPublish.Text = book.publish;
            this.Title = "Изменение книги";
        }
        // Добавление и Сохранение
        private void Save(object sender, RoutedEventArgs e)
        {
            if (ValidateValues())
            {
                _books.title = txtTitle.Text;
                _books.author = txtAuthor.Text;
                _books.style = txtStyle.Text;
                _books.publish = txtPublish.Text;
                if (_isBookEdit)
                {
                    var contactInDb = _context.books.Find(_books.id);

                    if (contactInDb != null)
                    {
                        _context.Entry(contactInDb).CurrentValues.SetValues(_books);

                        _context.SaveChanges();
                        DialogResult = true;
                    }
                }
                else
                {
                    if (_books != null)
                    {
                        _context.books.Add(_books);

                        _context.SaveChanges();
                        DialogResult = true;
                    }
                }
            }
        }

        // Проверка на правильность введенных данных
        private bool ValidateValues()
        {
            if (string.IsNullOrEmpty(txtTitle.Text) || txtTitle.Text.Length > 30)
            {
                MessageBox.Show("Поле не может быть пустым или содержать больше 30 символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtAuthor.Text) || txtAuthor.Text.Length > 50)
            {
                MessageBox.Show("Поле не может быть пустым или содержать больше 50 символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtStyle.Text) || txtStyle.Text.Length > 25)
            {
                MessageBox.Show("Поле не может быть пустым или содержать больше 25 символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtPublish.Text) || txtPublish.Text.Length > 30)
            {
                MessageBox.Show("Поле не может быть пустым или содержать больше 30 символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
