using System;
using System.Linq;
using System.Windows;

namespace WpfEconomicPlatform
{
    public partial class MainWindow : Window
    {
        private FinancialPlannerEntities db;

        public MainWindow()
        {
            InitializeComponent();
            db = new FinancialPlannerEntities();
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
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            var user = db.Users.FirstOrDefault(u => u.login == login && u.password == password);
            if (user != null)
            {
                IncomesOutcomes mainWindow = new IncomesOutcomes();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }
    }
}
