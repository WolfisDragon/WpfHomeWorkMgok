using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfEconomicPlatform
{
    public partial class addCategory : Window
    {
        private FinancialPlannerEntities db;

        public addCategory()
        {
            InitializeComponent();
            db = new FinancialPlannerEntities();
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            string categoryName = categoryNameTextBox.Text.Trim();
            string selectedType = (categoryTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(selectedType))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (selectedType == "Доход")
                {
                    if (db.CategoriesIncomes.Any(c => c.title == categoryName))
                    {
                        MessageBox.Show("Такая категория доходов уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    db.CategoriesIncomes.Add(new CategoriesIncome { title = categoryName });
                }
                else if (selectedType == "Расход")
                {
                    if (db.CategoriesOutcomes.Any(c => c.title == categoryName))
                    {
                        MessageBox.Show("Такая категория расходов уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    db.CategoriesOutcomes.Add(new CategoriesOutcome { title = categoryName });
                }

                db.SaveChanges();
                MessageBox.Show("Категория успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении категории: {ex.Message}\n\nПодробности: {ex.InnerException?.Message}",
                                 "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
