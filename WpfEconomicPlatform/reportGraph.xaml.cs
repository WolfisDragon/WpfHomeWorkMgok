using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace WpfEconomicPlatform
{
    public partial class reportGraph : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> Formatter { get; set; } = value => value.ToString("C0");

        public reportGraph()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection(); // Обязательно до привязки
            Labels = new List<string>();
            Formatter = value => value.ToString("C0");

            DataContext = this; // Устанавливаем контекст ДО загрузки данных

            LoadChartData(); // Подгружаем данные
        }


private void LoadChartData()
{
    var db = new FinancialPlannerIS322DEntities();
    int userId = CurrentUser.UserId;

    // Получаем агрегированные данные по доходам и расходам
    var rawOutcomes = db.Outcomes
        .Where(o => o.userId == userId)
        .GroupBy(o => new { o.date.Year, o.date.Month })
        .Select(g => new
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            Total = g.Sum(o => o.amount)
        })
        .ToList();

    var rawIncomes = db.Incomes
        .Where(i => i.userId == userId)
        .GroupBy(i => new { i.date.Year, i.date.Month })
        .Select(g => new
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            Total = g.Sum(i => i.amount)
        })
        .ToList();

    // Преобразуем в DateTime
    var outcomes = rawOutcomes.Select(x => new
    {
        Date = new DateTime(x.Year, x.Month, 1),
        x.Total
    }).ToList();

    var incomes = rawIncomes.Select(x => new
    {
        Date = new DateTime(x.Year, x.Month, 1),
        x.Total
    }).ToList();

    // Определяем диапазон месяцев
    var minDate = outcomes.Select(x => x.Date)
        .Concat(incomes.Select(x => x.Date))
        .DefaultIfEmpty(DateTime.Today)
        .Min();

    var maxDate = outcomes.Select(x => x.Date)
        .Concat(incomes.Select(x => x.Date))
        .DefaultIfEmpty(DateTime.Today)
        .Max();

    // Генерируем список всех месяцев в диапазоне
    var allDates = new List<DateTime>();
    for (var date = new DateTime(minDate.Year, minDate.Month, 1);
         date <= maxDate;
         date = date.AddMonths(1))
    {
        allDates.Add(date);
    }

    var outcomeValues = new ChartValues<double>();
    var incomeValues = new ChartValues<double>();
    Labels = new List<string>();

    foreach (var date in allDates)
    {
        var outcome = outcomes.FirstOrDefault(x => x.Date == date)?.Total ?? 0;
        var income = incomes.FirstOrDefault(x => x.Date == date)?.Total ?? 0;

        outcomeValues.Add((double)outcome);
        incomeValues.Add((double)income);
        Labels.Add(date.ToString("MMM yyyy"));
    }

    SeriesCollection = new SeriesCollection
    {
        new LineSeries
        {
            Title = "Расходы",
            Values = outcomeValues,
            Stroke = Brushes.IndianRed,
            Fill = Brushes.Transparent,
            StrokeThickness = 4,
            PointGeometrySize = 8
        },
        new LineSeries
        {
            Title = "Доходы",
            Values = incomeValues,
            Stroke = Brushes.ForestGreen,
            Fill = Brushes.Transparent,
            StrokeThickness = 4,
            PointGeometrySize = 8
        }
    };

    DataContext = this;
}




        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
