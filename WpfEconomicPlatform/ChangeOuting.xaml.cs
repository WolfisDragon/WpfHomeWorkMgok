using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfEconomicPlatform.entities;

namespace WpfEconomicPlatform
{
    /// <summary>
    /// Логика взаимодействия для ChangeCategory.xaml
    /// </summary>
    public partial class ChangeOuting : Window
    {
        private FinancialPlannerIS322DEntities db;
        private int userId;
        private IncomesOutcomesDTO ob;

        public ChangeOuting(IncomesOutcomesDTO selectedObject)
        {
            InitializeComponent();
            ob = selectedObject;
            userId = CurrentUser.UserId;
            db = new FinancialPlannerIS322DEntities();

            LoadData();
        }

        private void LoadData()
        {
            List<addIncomesOutcomes.Category> categories = new List<addIncomesOutcomes.Category>();

            if (ob.Type == "Расход")
            {
                categories = db.CategoriesOutcome
                    .Where(c => c.userId == userId)
                    .Select(c => new addIncomesOutcomes.Category { Id = c.id, Title = c.title })
                    .ToList();
            }
            else if (ob.Type == "Доход")
            {
                categories = db.CategoriesIncome
                    .Where(c => c.userId == userId)
                    .Select(c => new addIncomesOutcomes.Category { Id = c.id, Title = c.title })
                    .ToList();
            }

            if (categories.Any())
            {
                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.DisplayMemberPath = "Title";
                CategoryComboBox.SelectedValuePath = "Id";

                CategoryComboBox.SelectedIndex = 0;

                SummTextBox.Text = ob.Amount.ToString();
            }
        }

        private void exitChangeOuting(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void readyChangeOuting(object sender, RoutedEventArgs e)
        {
            saveChangeOuting();
            this.Close();
        }

        private void saveChangeOuting()
        {
            var id = ob.Id;
            var category = CategoryComboBox.Text;
            var amountText = SummTextBox.Text;
            var type = ob.Type;

            DateTime currentDate = DateTime.Now;

            try
            {
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
                    int categoryId = db.CategoriesIncome.Where(c => c.title == category).Select(c => c.id).FirstOrDefault();
                    if (categoryId == 0) throw new Exception("Категория дохода не найдена.");
                    
                    Incomes selectedIncome = db.Incomes.Where(i => i.id == id).FirstOrDefault();

                    selectedIncome.amount = amount;
                    selectedIncome.categoryId = categoryId;

                }
                else if (type == "Расход")
                {
                    int categoryId = db.CategoriesOutcome.Where(c => c.title == category).Select(c => c.id)
                        .FirstOrDefault();
                    if (categoryId == 0) throw new Exception("Категория расхода не найдена.");

                    Outcomes selectedOutcome = db.Outcomes.Where(o => o.id == id).FirstOrDefault();
                    
                    selectedOutcome.amount = amount;
                    selectedOutcome.categoryId = categoryId;
                }
                else
                {
                    MessageBox.Show("Выберите тип операции: Доход или Расход.");
                    return;
                }

                db.SaveChanges();
                MessageBox.Show("Операция успешно добавлена!");
            }catch (Exception ex) {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }
    }
}

