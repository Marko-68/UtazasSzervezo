using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Admin.Commands;
using System.Net.Http;
using System.Text.Json;
using UtazasSzervezo_Admin.Views;

namespace UtazasSzervezo_Admin.ViewModels
{
    public class AccommodationViewModel : BaseViewModel
    {
        public ObservableCollection<Accommodation> Accommodations { get; set; } = new();

        public ICommand LoadCommand { get; }
        public ICommand NewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5133/")
        };

        private Accommodation _selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get => _selectedAccommodation;
            set => SetProperty(ref _selectedAccommodation, value);
        }

        public AccommodationViewModel()
        {
            LoadCommand = new RelayCommand(async _ => await LoadAsync());
            NewCommand = new RelayCommand(_ => OpenEditor(null));
            EditCommand = new RelayCommand(_ => OpenEditor(SelectedAccommodation), _ => SelectedAccommodation != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => SelectedAccommodation != null);
            _ = LoadAsync();

        }

        private async Task LoadAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/Accommodation");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<List<Accommodation>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Accommodations.Clear();
                    foreach (var item in data!)
                        Accommodations.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a szállások betöltésekor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenEditor(Accommodation accommodation)
        {
            var editor = new Views.AccommodationEditorView(accommodation);
            if (editor.ShowDialog() == true)
            {
                _ = SaveAsync(editor.EditedAccommodation);
            }
        }

        private async Task SaveAsync(Accommodation accommodation)
        {
            try
            {
                var json = JsonSerializer.Serialize(accommodation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = accommodation.id == 0
                    ? await _http.PostAsync("api/Accommodation", content)
                    : await _http.PutAsync($"api/Accommodation/{accommodation.id}", content);

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
            if (SelectedAccommodation == null) return;
            var confirm = MessageBox.Show("Biztosan törlöd a szállást?", "Törlés megerősítése", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    var response = await _http.DeleteAsync($"api/Accommodation/{SelectedAccommodation.id}");
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
