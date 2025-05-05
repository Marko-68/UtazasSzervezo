using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Services;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace UtazasSzervezo_Admin.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5133/")
        };

        public SeriesCollection BookingsPerMonthSeries { get; set; } = new();
        public ObservableCollection<string> Months { get; set; } = new();

        public SeriesCollection RevenuePerMonthSeries { get; set; } = new();
        public Func<double, string> CurrencyFormatter => value => value.ToString("C0", new CultureInfo("hu-HU"));
        public Func<double, string> RevenuePerMonthLabels => value => value.ToString("C0", new CultureInfo("hu-HU"));

        public SeriesCollection PopularCitiesSeries { get; set; } = new();
        public ObservableCollection<string> CityLabels { get; set; } = new();
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public StatisticsViewModel()
        {
            _ = LoadAllAsync();
        }

        private async Task LoadAllAsync()
        {
            await Task.WhenAll(
                LoadBookingsPerMonthAsync(),
                LoadRevenuePerMonthAsync(),
                LoadPopularCitiesAsync()
            );
        }

        private async Task LoadBookingsPerMonthAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/statistics/bookings-per-month");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<MonthStat>>(json);

                BookingsPerMonthSeries.Clear();
                var values = new ChartValues<int>();

                foreach (var item in data)
                {
                    if (item.month >= 1 && item.month <= 12)
                    {
                        Months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.month));
                        values.Add(item.count);
                    }
                }
                //throw new Exception("Invalid month data");

                BookingsPerMonthSeries.Add(new ColumnSeries
                {
                    Title = "Foglalások",
                    Values = values
                });

                OnPropertyChanged(nameof(BookingsPerMonthSeries));
                OnPropertyChanged(nameof(Months));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba a foglalások statisztikánál: {ex.Message}";
            }
        }

        private async Task LoadRevenuePerMonthAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/statistics/revenue-per-month");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<MonthRevenueStat>>(json)!;

                RevenuePerMonthSeries.Clear();
                var values = new ChartValues<decimal>();

                foreach (var item in data)
                {
                    values.Add(item.total);
                }

                RevenuePerMonthSeries.Add(new ColumnSeries
                {
                    Title = "Bevétel (Ft)",
                    Values = values
                });

                OnPropertyChanged(nameof(RevenuePerMonthSeries));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba a bevétel statisztikánál: {ex.Message}";
            }
        }

        private async Task LoadPopularCitiesAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/statistics/popular-cities");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<CityStat>>(json)!;

                PopularCitiesSeries.Clear();
                CityLabels.Clear();

                var pieSeries = new SeriesCollection();
                foreach (var city in data)
                {
                    pieSeries.Add(new PieSeries
                    {
                        Title = city.city,
                        Values = new ChartValues<int> { city.count },
                        DataLabels = true
                    });
                    CityLabels.Add(city.city);
                }

                PopularCitiesSeries = pieSeries;
                OnPropertyChanged(nameof(PopularCitiesSeries));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba a népszerű városok statisztikánál: {ex.Message}";
            }
        }

        //public record MonthStat(int Month, int Count);
        private record MonthRevenueStat(int month, decimal total);
        private record CityStat(string city, int count);
        public class MonthStat()
        {
            public int month { get; set; }
            public int count { get; set; }
        }
    }
}
