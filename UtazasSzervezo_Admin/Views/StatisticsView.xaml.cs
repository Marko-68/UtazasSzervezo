using LiveCharts.Wpf;
using LiveCharts;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtazasSzervezo_Admin.Views
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();
            //LoadStatistics();
        }

        private void LoadStatistics()
        {
            var revenues = new List<MonthlyRevenue>
                {
                    new MonthlyRevenue { Month = "January", Revenue = 1200 },
                    new MonthlyRevenue { Month = "February", Revenue = 1450 },
                    new MonthlyRevenue { Month = "March", Revenue = 1700 },
                    new MonthlyRevenue { Month = "April", Revenue = 2100 }
                };

            var revenueValues = new ChartValues<double>(revenues.Select(r => r.Revenue));
            var labels = revenues.Select(r => r.Month).ToArray();

            RevenueChart.Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Revenue",
                        Values = revenueValues
                    }
                };
        }
    }

    public class MonthlyRevenue
    {
        public string Month { get; set; }
        public double Revenue { get; set; }
    }
}

