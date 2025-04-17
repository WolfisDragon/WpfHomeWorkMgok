using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace WpfEconomicPlatform
{
    public partial class addCategory : Window
    {
        private int userId;
        private FinancialPlannerIS322DEntities db;

        public addCategory(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            db = new FinancialPlannerIS322DEntities();
        }

        private void exitAddCategory (object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void createCategory(object sender, RoutedEventArgs e)
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
                    
                    if (db.CategoriesIncome.Any(c => c.title == categoryName && c.userId == userId))
                    {
                        MessageBox.Show("Такая категория доходов уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    
                    db.CategoriesIncome.Add(new CategoriesIncome { title = categoryName, userId = userId });
                }
                else if (selectedType == "Расход")
                {
                    
                    if (db.CategoriesOutcome.Any(c => c.title == categoryName && c.userId == userId))
                    {
                        MessageBox.Show("Такая категория расходов уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                
                    db.CategoriesOutcome.Add(new CategoriesOutcome { title = categoryName, userId = userId });
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
