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

namespace UtazasSzervezo_Admin.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; } = new();

        public ICommand LoadCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5133/")
        };

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public UserViewModel()
        {
            LoadCommand = new RelayCommand(async _ => await LoadAsync());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => SelectedUser != null);
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/users");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Users.Clear();
                    foreach (var user in data!)
                        Users.Add(user);
                }
                else
                {
                    MessageBox.Show("Nem sikerült betölteni a felhasználókat.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Betöltési hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedUser == null) return;

            var confirm = MessageBox.Show($"Biztosan törlöd a(z) {SelectedUser.UserName} felhasználót?", "Törlés megerősítése", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    var response = await _http.DeleteAsync($"api/users/{SelectedUser.Id}");
                    if (response.IsSuccessStatusCode)
                        await LoadAsync();
                    else
                        MessageBox.Show("A törlés nem sikerült.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Törlési hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
