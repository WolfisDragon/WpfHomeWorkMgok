using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfEconomicPlatform
{
    public partial class addCategory : Window
    {
        private int userId; // Сохраняем userId
        private FinancialPlannerEntities db; // Экземпляр контекста базы данных

        // Конструктор, принимающий userId
        public addCategory(int userId)
        {
            InitializeComponent();
            this.userId = userId; // Сохраняем userId
            db = new FinancialPlannerEntities(); // Инициализируем контекст базы данных
        }

        // Метод для создания новой категории
        private void createCategory(object sender, RoutedEventArgs e)
        {
            // Получаем название категории из поля ввода
            string categoryName = categoryNameTextBox.Text.Trim();
            // Получаем тип категории (Доход или Расход)
            string selectedType = (categoryTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(selectedType))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Обработка типа категории: Доход или Расход
                if (selectedType == "Доход")
                {
                    // Проверяем, существует ли категория с таким названием у текущего пользователя
                    if (db.CategoriesIncomes.Any(c => c.title == categoryName && c.userId == userId))
                    {
                        MessageBox.Show("Такая категория доходов уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Добавляем новую категорию дохода
                    db.CategoriesIncomes.Add(new CategoriesIncome { title = categoryName, userId = userId });
                }
                else if (selectedType == "Расход")
                {
                    // Проверяем, существует ли категория с таким названием у текущего пользователя
                    if (db.CategoriesOutcomes.Any(c => c.title == categoryName && c.userId == userId))
                    {
                        MessageBox.Show("Такая категория расходов уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Добавляем новую категорию расхода
                    db.CategoriesOutcomes.Add(new CategoriesOutcome { title = categoryName, userId = userId });
                }

                // Сохраняем изменения в базе данных
                db.SaveChanges();
                // Сообщаем об успехе
                MessageBox.Show("Категория успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Закрываем окно
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                // Обрабатываем ошибки
                MessageBox.Show($"Ошибка при добавлении категории: {ex.Message}\n\nПодробности: {ex.InnerException?.Message}",
                                 "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
