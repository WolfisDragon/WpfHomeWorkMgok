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

namespace WpfEconomicPlatform
{
    /// <summary>
    /// Логика взаимодействия для IncomesOutcomes.xaml
    /// </summary>
    public partial class IncomesOutcomes : Window
    {
        public IncomesOutcomes(int userId)
        {
            InitializeComponent();
        }

        private void addIndOut (object send, RoutedEventArgs e)
        {
            addIncomesOutcomes addIncOutWindow = new addIncomesOutcomes(CurrentUser.UserId);
            addIncOutWindow.ShowDialog();
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
    }
}
