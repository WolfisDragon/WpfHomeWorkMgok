using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfEconomicPlatform
{
    public partial class addIncomesOutcomes : Window
    {
        private FinancialPlannerIS322DEntities db;
        private int userId;

        public addIncomesOutcomes(int currentUserId)
        {
            MessageBox.Show("Конструктор вызывается!");
            InitializeComponent();
            db = new FinancialPlannerIS322DEntities();
            userId = currentUserId;
        }
        public static List<string> Items { get; } = new List<string>

        {
            "Доход",
            "Расход"
        };

        public class Category
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }

        private void LoadCategories(string transactionType)
        {
            try
            {
                MessageBox.Show("Загрузка категорий началась.");

                categoryComboBox.ItemsSource = null;

                List<Category> categories = new List<Category>();

                if (transactionType == "Доход")
                {

                    categories = db.CategoriesIncome
                        .Where(c => c.userId == userId)
                        .Select(c => new Category { Id = c.id, Title = c.title })
                        .ToList();
                }
                else if (transactionType == "Расход")
                {
                    categories = db.CategoriesOutcome
                        .Where(c => c.userId == userId)
                        .Select(c => new Category { Id = c.id, Title = c.title })
                        .ToList();
                }

                MessageBox.Show($"Загружено категорий: {categories.Count}");

                if (categories.Any())
                {
                    categoryComboBox.ItemsSource = categories;
                    categoryComboBox.DisplayMemberPath = "Title";
                    categoryComboBox.SelectedValuePath = "Id";

                    categoryComboBox.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Нет доступных категорий для пользователя.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки категорий: " + ex.Message);
            }
        }

        private void transactionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedType = transactionTypeComboBox.SelectedItem.ToString();
            
            MessageBox.Show(selectedType);

            if (!string.IsNullOrEmpty(selectedType))
            {
                LoadCategories(selectedType);
            }
        }

        private void send_IncomesOrOutcomes(object sender, RoutedEventArgs e)
        {
            try
            {
                string type = transactionTypeComboBox.Text;
                string category = categoryComboBox.Text;
                string amountText = amountTextBox.Text;
                string description = descriptionTextBox.Text;
                DateTime currentDate = DateTime.Now;

                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(amountText))
                {
                    MessageBox.Show("Заполните все поля.");
                    return;
                }

                if (!int.TryParse(amountText, out int amount) || amount <= 0)
                {
                    MessageBox.Show("Сумма должна быть положительным числом.");
                    return;
                }

                if (type == "Доход")
                {
                    int categoryId = db.CategoriesIncome.Where(c=> c.title == category).Select(c=>c.id).FirstOrDefault();
                    if (categoryId == 0) throw new Exception("Категория дохода не найдена.");

                    Incomes income = new Incomes
                    {
                        userId = userId,
                        amount = amount,
                        date = currentDate,
                        categoryId = categoryId,
                        description = description
                    };
                    db.Incomes.Add(income);
                }
                else if (type == "Расход")
                {
                    int categoryId = db.CategoriesOutcome.Where(c=> c.title == category).Select(c=>c.id).FirstOrDefault();
                    if (categoryId == 0) throw new Exception("Категория расхода не найдена.");
                
                    Outcomes outcome = new Outcomes
                    {
                        userId = userId,
                        amount = amount,
                        date = currentDate,
                        categoryId = categoryId,
                        description = description
                    };
                    db.Outcomes.Add(outcome);
                }
                else
                {
                    MessageBox.Show("Выберите тип операции: Доход или Расход.");
                    return;
                }

                db.SaveChanges();
                MessageBox.Show("Операция успешно добавлена!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }
    }
}
