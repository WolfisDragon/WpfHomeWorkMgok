using System;
using System.Linq;
using System.Windows;

namespace WpfEconomicPlatform
{
    public partial class categoryList : Window
    {
        private FinancialPlannerEntities db;
        private int currentUserId; // Текущий ID пользователя

        public categoryList(int userId)
        {
            InitializeComponent();
            db = new FinancialPlannerEntities();
            currentUserId = userId; // Устанавливаем ID текущего пользователя
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                // Загружаем категории для текущего пользователя
                var categories = db.CategoriesIncomes
                    .Where(c => c.userId == currentUserId) // Фильтруем по userId
                    .Select(c => new { Название = c.title, Тип = "Доход" })
                    .Union(
                        db.CategoriesOutcomes
                            .Where(c => c.userId == currentUserId) // Фильтруем по userId
                            .Select(c => new { Название = c.title, Тип = "Расход" })
                    )
                    .ToList();

                MessageBox.Show($"Загружено {categories.Count} категорий для пользователя с ID {currentUserId}");

                categoryGrid.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки категорий: " + ex.Message);
            }
        }

        private void addCategorys(object sender, RoutedEventArgs e)
        {
            addCategory addCategoryWindow = new addCategory();
            if (addCategoryWindow.ShowDialog() == true)
            {
                LoadCategories();
            }
        }
    }
}
