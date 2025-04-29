using System;
using System.Linq;
using System.Windows;
using WpfEconomicPlatform.entities;

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

                var incomeCategories = db.CategoriesIncome
                    .Where(c => c.userId == currentUserId)
                    .Select(c => new CategoryDto
                    {
                        Название = c.title,
                        Тип = "Доход",
                        Id = c.id,
                        UserId = c.userId
                    });

                var outcomeCategories = db.CategoriesOutcome
                    .Where(c => c.userId == currentUserId)
                    .Select(c => new CategoryDto
                    {
                        Название = c.title,
                        Тип = "Расход",
                        Id = c.id,
                        UserId = c.userId
                    });

                var categories = incomeCategories.Union(outcomeCategories).ToList();

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

        //КНОПКА УДАЛИТЬ
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            CategoryDto selectedCategory = categoryGrid.SelectedItem as CategoryDto;
            if (selectedCategory == null)
            {
                MessageBox.Show("Выберите категорию для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Вы действительно хотите удалить категорию \"{selectedCategory.Название}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                if (selectedCategory.Тип == "Доход")
                {
                    var category = db.CategoriesIncome.FirstOrDefault(c => c.id == selectedCategory.Id && c.userId == selectedCategory.UserId);
                    if (category != null)
                    {
                        db.CategoriesIncome.Remove(category);
                    }
                }
                else if (selectedCategory.Тип == "Расход")
                {
                    var category = db.CategoriesOutcome.FirstOrDefault(c => c.id == selectedCategory.Id && c.userId == selectedCategory.UserId);
                    if (category != null)
                    {
                        db.CategoriesOutcome.Remove(category);
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Категория удалена успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении категории: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //КНОПКА ИЗМЕНИТЬ
        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedCategory = categoryGrid.SelectedItem;
            if (selectedCategory == null)
            {
                MessageBox.Show("Выберите категорию для изменения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ChangeCategory changeCategoryWindow = new ChangeCategory(
                selectedCategory.Id,
                selectedCategory.UserId,
                selectedCategory.Название
            );
            changeCategoryWindow.ShowDialog();
            LoadCategories(); 
        }


    }
}