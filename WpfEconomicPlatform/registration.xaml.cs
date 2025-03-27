using System;
using System.Linq;
using System.Windows;

namespace WpfEconomicPlatform
{
    public partial class registration : Window
    {
        private FinancialPlannerEntities db;

        public registration()
        {
            InitializeComponent();
            db = new FinancialPlannerEntities();
        }

        private void Registration(object sender, RoutedEventArgs e)
        {
            string fullName = fullNameTextBox.Text.Trim();
            string email = emailTextBox.Text.Trim();
            string login = loginTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (db.Users.Any(u => u.login == login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует.");
                return;
            }

            if (db.Users.Any(u => u.email == email))
            {
                MessageBox.Show("Пользователь с такой почтой уже зарегистрирован.");
                return;
            }

            try
            {

                User newUser = new User
                {
                    fullname = fullName,
                    login = login,
                    password = password,
                    email = email
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                int userId = newUser.id;

                db.CategoriesIncomes.AddRange(new[]
                {
                    new CategoriesIncome { userId = userId, title = "Salary" },
                    new CategoriesIncome { userId = userId, title = "Bonuses" },
                    new CategoriesIncome { userId = userId, title = "Additional income" }
                });

                db.CategoriesOutcomes.AddRange(new[]
                {
                    new CategoriesOutcome { userId = userId, title = "Groceries" },
                    new CategoriesOutcome { userId = userId, title = "Transportation" },
                    new CategoriesOutcome { userId = userId, title = "Utilities" },
                    new CategoriesOutcome { userId = userId, title = "Entertainment" }
                });

                db.SaveChanges();

                MessageBox.Show("Регистрация успешна! Стандартные категории добавлены.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка регистрации: " + ex.Message);
            }
        }
    }
}
