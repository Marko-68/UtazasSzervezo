using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;
using UtazasSzervezo_Admin.Commands;
using System.Net.Http;
using System.Text.Json;

namespace UtazasSzervezo_Admin.ViewModels
{
    public class FlightViewModel : BaseViewModel
    {
        public ObservableCollection<Flight> Flights { get; set; } = new();

        public ICommand LoadCommand { get; }
        public ICommand NewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7258/") };

        private Flight _selectedFlight;
        public Flight SelectedFlight
        {
            get => _selectedFlight;
            set => SetProperty(ref _selectedFlight, value);
        }

        public FlightViewModel()
        {
            LoadCommand = new RelayCommand(async _ => await LoadAsync());
            NewCommand = new RelayCommand(_ => OpenEditor(null));
            EditCommand = new RelayCommand(_ => OpenEditor(SelectedFlight), _ => SelectedFlight != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => SelectedFlight != null);
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/flights");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<List<Flight>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Flights.Clear();
                    foreach (var item in data!)
                        Flights.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenEditor(Flight flight)
        {
            var editor = new Views.FlightEditorView(flight);
            if (editor.ShowDialog() == true)
            {
                _ = SaveAsync(editor.EditedFlight);
            }
        }

        private async Task SaveAsync(Flight flight)
        {
            try
            {
                var json = JsonSerializer.Serialize(flight);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = flight.id == 0
                    ? await _http.PostAsync("api/flights", content)
                    : await _http.PutAsync($"api/flights/{flight.id}", content);

                if (response.IsSuccessStatusCode)
                    await LoadAsync();
                else
                    MessageBox.Show("Mentés sikertelen.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mentési hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedFlight == null) return;
            var confirm = MessageBox.Show("Biztosan törlöd a járatot?", "Törlés megerősítése", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    var response = await _http.DeleteAsync($"api/flights/{SelectedFlight.id}");
                    if (response.IsSuccessStatusCode)
                        await LoadAsync();
                    else
                        MessageBox.Show("Törlés sikertelen.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Törlési hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
