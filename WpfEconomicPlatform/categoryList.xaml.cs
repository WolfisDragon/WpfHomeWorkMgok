using System;
using System.Linq;
using System.Windows;

namespace WpfEconomicPlatform
{
    public partial class categoryList : Window
    {
        private FinancialPlannerEntities db;

        public categoryList()
        {
            InitializeComponent();
            db = new FinancialPlannerEntities();
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
               
                int currentUserId = CurrentUser.UserId;

                var categories = db.CategoriesIncomes
                    .Where(c => c.userId == currentUserId)
                    .Select(c => new { Название = c.title, Тип = "Доход" })
                    .Union(
                        db.CategoriesOutcomes
                            .Where(c => c.userId == currentUserId)
                            .Select(c => new { Название = c.title, Тип = "Расход" })
                    )
                    .ToList();

                categoryGrid.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки категорий: " + ex.Message);
            }
        }

        private void showAddCategoryWindow(object sender, RoutedEventArgs e)
        {
            addCategory addCategoryWindow = new addCategory(CurrentUser.UserId);
            addCategoryWindow.ShowDialog(); 
            LoadCategories(); 
        }
    }
}