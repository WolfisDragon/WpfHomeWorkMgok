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
    /// Логика взаимодействия для Setgoals.xaml
    /// </summary>
    public partial class Setgoals : Window
    {
        public Setgoals()
        {
            InitializeComponent();
        }

        public void exit (object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
