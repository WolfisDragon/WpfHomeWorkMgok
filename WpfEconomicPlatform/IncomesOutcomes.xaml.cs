using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfEconomicPlatform.entities;

namespace WpfEconomicPlatform
{
    /// <summary>
    /// Логика взаимодействия для IncomesOutcomes.xaml
    /// </summary>
    public partial class IncomesOutcomes : Window
    {
        private FinancialPlannerIS322DEntities entities;
        private int currentUser = CurrentUser.UserId;
        
        public IncomesOutcomes()
        {
            InitializeComponent();
            entities = new FinancialPlannerIS322DEntities();
            SelectData();
        }

        void SelectData()
        {
            DataGridIncomesOutcomes.ItemsSource = null;
            var allData = entities
                .Users
                .Include(u => u.Incomes)
                .Include(u => u.Outcomes)
                .Include(u => u.IncomeBudgetSettings)
                .Include(u => u.OutcomeBudgetSettings)
                .Include(u => u.CategoriesIncome)
                .Include(u => u.CategoriesOutcome)
                .Where(u => u.id == currentUser)
                .ToList();

            var userData = allData.FirstOrDefault();
            if (userData != null)
            {
                var result = new List<IncomesOutcomesDTO>();

                result.AddRange(userData.Incomes.Select(i => new IncomesOutcomesDTO
                {
                    Id = i.id,
                    Amount = i.amount,
                    Date = i.date.ToString("dd/MM/yyyy HH:mm"),
                    Category = i.CategoriesIncome.title,
                    Type = "Доход"
                }));

                result.AddRange(userData.Outcomes.Select(o => new IncomesOutcomesDTO
                {
                    Id = o.id,
                    Amount = o.amount,
                    Date = o.date.ToString("dd/MM/yyyy HH:mm"),
                    Category = o.CategoriesOutcome.title,
                    Type = "Расход"
                }));

                var incomeSetting = userData.IncomeBudgetSettings
                    .Where(s => s.userId == currentUser && s.dateEnd > DateTime.Now)
                    .OrderByDescending(s => s.dateStart)
                    .FirstOrDefault();

                var outcomeSetting = userData.OutcomeBudgetSettings
                    .Where(s => s.userId == currentUser && s.dateEnd > DateTime.Now)
                    .OrderByDescending(s => s.dateStart)
                    .FirstOrDefault();

                var income = (incomeSetting != null)
                    ? userData.Incomes.Where(i => i.date >= incomeSetting.dateStart).Sum(i => i.amount)
                    : userData.Incomes.Sum(i => i.amount);

                var outcome = (outcomeSetting != null)
                    ? userData.Outcomes.Where(o => o.date >= outcomeSetting.dateStart).Sum(o => o.amount)
                    : userData.Outcomes.Sum(o => o.amount);

                var balance = income - outcome;

                TextBlockIncomes.Text = $"{income}/{(incomeSetting != null ? incomeSetting.totalAmount + "\u20bd" : "—")}";
                TextBlockOutcomes.Text = $"{outcome}/{(outcomeSetting != null ? outcomeSetting.totalAmount + "\u20bd" : "—")}";
                TextBlockBalance.Text = $"{balance}\u20bd";

                UserName.Text = userData.fullname;
                DataGridIncomesOutcomes.ItemsSource = result;

                // if (incomeSetting != null && income >= incomeSetting.totalAmount)
                // {
                //     MessageBox.Show("Поздравляем! Цель по доходам достигнута!", "Цель достигнута", MessageBoxButton.OK, MessageBoxImage.Information);
                // }
                //
                // if (outcomeSetting != null && outcome >= outcomeSetting.totalAmount)
                // {
                //     MessageBox.Show("Внимание! Вы превысили лимит по расходам.", "Превышение цели", MessageBoxButton.OK, MessageBoxImage.Warning);
                // }
            }
        }
        
        private void addIndOut (object send, RoutedEventArgs e)
        {
            addIncomesOutcomes addIncOutWindow = new addIncomesOutcomes(CurrentUser.UserId);
            addIncOutWindow.ShowDialog();
            SelectData();
        }

        private void categoryList (object send, RoutedEventArgs e)
        {
            categoryList categoryListWindow = new categoryList();
            categoryListWindow.ShowDialog();
        }

        private void report (object send, RoutedEventArgs e)
        {
            reportGraph reportGraphWindow = new reportGraph();
            reportGraphWindow.ShowDialog();
        }

        private void SetGoals (object send, RoutedEventArgs e)
        {
            Setgoals SetGoalsWindow = new Setgoals(CurrentUser.UserId);
            SetGoalsWindow.ShowDialog();
        }
 
        private void exit(object send, RoutedEventArgs e) 
        { 
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = (IncomesOutcomesDTO)DataGridIncomesOutcomes.SelectedItem;

            if (selected.Type == "Расход")
            {
                var outcome = entities.Outcomes.FirstOrDefault(i => i.id == selected.Id);
                entities.Outcomes.Remove(outcome);
                entities.SaveChanges();
            }
            else if(selected.Type == "Доход")
            {
                var income = entities.Incomes.FirstOrDefault(i => i.id == selected.Id);
                entities.Incomes.Remove(income);
                entities.SaveChanges();
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
            SelectData();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            IncomesOutcomesDTO ob = (IncomesOutcomesDTO)DataGridIncomesOutcomes.SelectedItem;
            ChangeOuting changeOutingWindow = new ChangeOuting(ob);
            changeOutingWindow.ShowDialog();
            SelectData();
        }
    }
}
