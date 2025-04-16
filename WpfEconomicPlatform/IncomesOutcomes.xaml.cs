using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Runtime.Serialization;
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
            var allData = entities
                .Users
                .Include(u => u.Incomes)
                .Include(u => u.Outcomes)
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

                var userName = userData.fullname;
                var balance = userData.Incomes.Sum(i => i.amount) - userData.Outcomes.Sum(i => i.amount);
                
                TextBlockBalance.Text = balance.ToString();
                UserName.Text = userName;
                DataGridIncomesOutcomes.ItemsSource = result;
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
            Setgoals SetGoalsWindow = new Setgoals();
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
            throw new NotImplementedException();
        }
    }
}
