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
using System.Xml.Linq;

namespace WpfEconomicPlatform
{
    /// <summary>
    /// Логика взаимодействия для Setgoals.xaml
    /// </summary>
    public partial class Setgoals : Window
    {
        private int userId;
        private FinancialPlannerIS322DEntities db;

        public Setgoals(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            db = new FinancialPlannerIS322DEntities();

        }

        public void exit (object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string GoalIncome = setGoalIncome.Text.Trim();
            string GoalOutcome = setGoalOutcome.Text.Trim();
            DateTime DateStart = DateTime.Now;
            DateTime DateEnd = DateStart.AddDays(30);

            if (!int.TryParse(GoalIncome, out int goalIncome) || goalIncome <= 0)
            {
                MessageBox.Show("Значение цели должно быть положительным числом.");
                return;
            }

            if (!int.TryParse(GoalOutcome, out int goalOutcome) || goalOutcome <= 0)
            {
                MessageBox.Show("Значение цели должно быть положительным числом.");
                return;
            }

            try
            {

                db.OutcomeBudgetSettings.Add(new OutcomeBudgetSettings { userId = userId, totalAmount = goalOutcome, dateStart = DateStart, dateEnd = DateEnd, categoryOutcomeId = 1 });
                db.IncomeBudgetSettings.Add(new IncomeBudgetSettings { userId = userId, totalAmount = goalIncome, dateStart = DateStart, dateEnd = DateEnd, categoryIncomeId = 1 });

                db.SaveChanges();
                MessageBox.Show("Цели успешно добавлены!");
                this.Close();
            }
            catch
            {

            }
        }
    }

    }

