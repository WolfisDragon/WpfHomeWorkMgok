using System;
using System.Linq;
using System.Windows;

namespace WpfEconomicPlatform
{
    public partial class MainWindow : Window
    {
        private FinancialPlannerIS322DEntities db;

        public MainWindow()
        {
            InitializeComponent();
            db = new FinancialPlannerIS322DEntities();
        }

        private void openRegistration(object sender, RoutedEventArgs e)
        {
            registration registrationWindow = new registration();
            registrationWindow.ShowDialog();
        }

        private void Auth(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return;
            }

            var user = db.Users.FirstOrDefault(u => u.login == login && u.password == password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
            else
            {
                CurrentUser.UserId = user.id;

                // Передаем currentUserId в IncomesOutcomes
                IncomesOutcomes incomesOutcomesWindow = new IncomesOutcomes(CurrentUser.UserId);
                incomesOutcomesWindow.Show();
                this.Close();
            }
        }

    }

    public static class CurrentUser
    {
        public static int UserId { get; set; }
    }


}
