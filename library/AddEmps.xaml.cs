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
    /// Логика взаимодействия для AddEmps.xaml
    /// </summary>
    public partial class AddEmps : Window
    {
        private readonly Model1 _context;
        private bool _isUserEdit = false;
        private employees _emps;
        private users _users;
        public AddEmps(Model1 context)
        {
            InitializeComponent();
            _context = context;
            _users = new users();
            _emps = new employees();
            this.Title = "Добавление сотрудника";
        }
        public AddEmps(Model1 context, employees emps, users user) : this(context)
        {
            _isUserEdit = true;
            _emps = emps;
            txtLog.IsEnabled = false;
            txtPass.IsEnabled = false;
            txtLog.Text = user.login;
            txtPass.Text = user.password;
            txtName.Text = emps.first_name;
            txtLast.Text = emps.last_name;
            txtPost.Text = emps.post;
            txtDept.Text = emps.dept;
            this.Title = "Изменение сотрудника";
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            _users.login = txtLog.Text;
            _users.password = txtPass.Text;
            if (ValidateValues())
            {
                _emps.first_name = txtName.Text;
                _emps.last_name = txtLast.Text;
                _emps.post = txtPost.Text;
                _emps.dept = txtDept.Text;
                if (_isUserEdit)
                {
                    var empsInDb = _context.employees.Find(_emps.id);
                    var userInDb = _context.users.Where(p => p.id == _emps.id);

                    if (empsInDb != null) //userInDb != null
                    {
                        _context.Entry(empsInDb).CurrentValues.SetValues(_emps);
                        _context.SaveChanges();

                       // _context.Entry(userInDb).CurrentValues.SetValues(_users); //Ошибка

                        //_context.SaveChanges();
                        DialogResult = true;
                    }
                }
                else
                {
                    if (_emps != null && _users != null)
                    {
                        _context.users.Add(_users);
                        _users.access = 2;
                        _emps.account_id = _users.id;
                        _context.employees.Add(_emps);

                        _context.SaveChanges();
                        DialogResult = true;
                    }
                }
            }


        }
        private bool ValidateValues()
        {
            if (string.IsNullOrEmpty(txtPost.Text) || txtPost.Text.Length > 20)
            {
                MessageBox.Show("Поле не может быть пустым или содержать больше 20 символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtDept.Text) || txtDept.Text.Length > 25)
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

            if (_context.users.FirstOrDefault(p => p.login == _users.login) != null && _isUserEdit == false)
            {
                MessageBox.Show("Такой пользователь уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
