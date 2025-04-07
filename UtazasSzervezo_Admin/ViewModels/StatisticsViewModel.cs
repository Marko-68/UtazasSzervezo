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
            BaseAddress = new Uri("https://localhost:7258/") 
        };

        public SeriesCollection BookingsPerMonthSeries { get; set; } = new();
        public string[] Months { get; set; } = Array.Empty<string>();

        public SeriesCollection RevenuePerMonthSeries { get; set; } = new();
        public Func<double, string> CurrencyFormatter => value => value.ToString("C0", new CultureInfo("hu-HU"));
        public Func<double, string> RevenuePerMonthLabels => value => value.ToString("C0", new CultureInfo("hu-HU"));

        public SeriesCollection PopularCitiesSeries { get; set; } = new();
        public ObservableCollection<string> CityLabels { get; set; } = new();

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
                var data = JsonSerializer.Deserialize<List<MonthStat>>(json)!;

                BookingsPerMonthSeries.Clear();
                Months = new string[data.Count];
                var values = new ChartValues<int>();

                for (int i = 0; i < data.Count; i++)
                {
                    Months[i] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(data[i].Month);
                    values.Add(data[i].Count);
                }

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
                MessageBox.Show($"Hiba a foglalások statisztikánál: {ex.Message}");
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
                    values.Add(item.Total);

                RevenuePerMonthSeries.Add(new ColumnSeries
                {
                    Title = "Bevétel (Ft)",
                    Values = values
                });

                OnPropertyChanged(nameof(RevenuePerMonthSeries));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a bevétel statisztikánál: {ex.Message}");
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
                        Title = city.City,
                        Values = new ChartValues<int> { city.Count },
                        DataLabels = true
                    });
                    CityLabels.Add(city.City);
                }

                PopularCitiesSeries = pieSeries;
                OnPropertyChanged(nameof(PopularCitiesSeries));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a népszerű városok statisztikánál: {ex.Message}");
            }
        }

        private record MonthStat(int Month, int Count);
        private record MonthRevenueStat(int Month, decimal Total);
        private record CityStat(string City, int Count);
    }
}

