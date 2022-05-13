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
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private readonly Model1 _context;
        private bool _isUserEdit = false;
        private readers _readers;
        private users _users;
        public AddUser(Model1 context)
        {
            InitializeComponent();
            _context = context;
            _users = new users();
            _readers = new readers();
            this.Title = "Добавление читателя";
        }
        public AddUser(Model1 context, readers reader, users user) : this(context)
        {
            _isUserEdit = true;
            _readers = reader;
            txtLog.IsEnabled = false;
            txtPass.IsEnabled = false;
            txtLog.Text = user.login;
            txtPass.Text = user.password;
            txtName.Text = reader.first_name;
            txtLast.Text = reader.last_name;
            this.Title = "Редактирование читателя";
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            _users.login = txtLog.Text;
            _users.password = txtPass.Text;
            if (ValidateValues())
            {
                _readers.first_name = txtName.Text;
                _readers.last_name = txtLast.Text;
                if (_isUserEdit)
                {
                    var readerInDb = _context.readers.Find(_readers.id);
                    //var userInDb = _context.users.Where(p=>p.id==_readers.id);

                    if (readerInDb != null ) //userInDb != null
                    {
                        _context.Entry(readerInDb).CurrentValues.SetValues(_readers);
                        _context.SaveChanges();

                        //_context.Entry(userInDb).CurrentValues.SetValues(_users); //Ошибка

                        //_context.SaveChanges();
                        DialogResult = true;
                    }
                }
                else
                {
                    if (_readers != null && _users != null)
                    {
                        _context.users.Add(_users);
                        _users.access = 1;
                        _readers.account_id = _users.id;
                        _context.readers.Add(_readers);

                        _context.SaveChanges();
                        DialogResult = true;
                    }

                }
            }


        }
        private bool ValidateValues()
        {
            if (string.IsNullOrEmpty(txtLog.Text) || txtLog.Text.Length > 20)
            {
                MessageBox.Show("Поле не может быть пустым или содержать больше 20 символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtPass.Text) || txtPass.Text.Length > 24)
            {
                MessageBox.Show("Поле не может быть пустым или содержать больше 24 символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtName.Text) || txtName.Text.Length > 30)
            {
                MessageBox.Show("Поле не может быть пустым или содержать больше 30 символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtLast.Text) || txtLast.Text.Length > 30)
            {
                MessageBox.Show("Поле не может быть пустым или содержать больше 30 символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (_context.users.FirstOrDefault(p=>p.login == _users.login) != null && _isUserEdit == false)
            {
                MessageBox.Show("Такой пользователь уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
