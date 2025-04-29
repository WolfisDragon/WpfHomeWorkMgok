using System;
using System.Linq;
using System.Windows;

namespace WpfEconomicPlatform
{
    public partial class ChangeCategory : Window
    {
        private FinancialPlannerIS322DEntities db;
        private int categoryId;
        private int userId; 

        public ChangeCategory(int categoryId, int userId, string currentTitle)
        {
            InitializeComponent();
            db = new FinancialPlannerIS322DEntities();
            this.categoryId = categoryId;
            this.userId = userId;

            inputName.Text = currentTitle;
        }

        private void renameCategory(object sender, RoutedEventArgs e)
        {
            string newCategoryName = inputName.Text.Trim();

            if (string.IsNullOrEmpty(newCategoryName))
            {
                MessageBox.Show("Введите новое название категории!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var incomeCategory = db.CategoriesIncome.FirstOrDefault(c => c.id == categoryId && c.userId == userId);

                if (incomeCategory != null)
                {
                    incomeCategory.title = newCategoryName;
                }
                else
                {
                    var outcomeCategory = db.CategoriesOutcome.FirstOrDefault(c => c.id == categoryId && c.userId == userId);

                    if (outcomeCategory != null)
                    {
                        outcomeCategory.title = newCategoryName;
                    }
                    else
                    {
                        MessageBox.Show("Категория не найдена или нет прав на её изменение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Категория успешно переименована!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переименовании: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void exitChangeCategory(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}

