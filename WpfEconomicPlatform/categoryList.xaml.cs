using System;
using System.Linq;
using System.Windows;

namespace WpfEconomicPlatform
{
    public partial class categoryList : Window
    {
        private FinancialPlannerIS322DEntities db;

        public categoryList()
        {
            InitializeComponent();
            db = new FinancialPlannerIS322DEntities();
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
               
                int currentUserId = CurrentUser.UserId;

                var categories = db.CategoriesIncome
                    .Where(c => c.userId == currentUserId)
                    .Select(c => new { Название = c.title, Тип = "Доход" })
                    .Union(
                        db.CategoriesOutcome
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

        private void exit (object sender, EventArgs e)
        {
            this.Close();
        }
    }
}