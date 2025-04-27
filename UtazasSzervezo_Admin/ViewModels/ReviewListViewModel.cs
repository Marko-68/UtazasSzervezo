using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UtazasSzervezo_Admin.Commands;
using UtazasSzervezo_Library.Models;
using System.Text.Json;

namespace UtazasSzervezo_Admin.ViewModels
{
    public class ReviewListViewModel:BaseViewModel
    {
        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        private Review _selectedReview;
        public Review SelectedReview
        {
            get => _selectedReview;
            set
            {
                _selectedReview = value;
                OnPropertyChanged(nameof(SelectedReview));
            }
        }

        public ICommand DeleteReviewCommand { get; }

        public ReviewListViewModel()
        {
            DeleteReviewCommand = new RelayCommand(async _ => await DeleteReview(), _ => SelectedReview != null);
            LoadReviews();
        }

        private async void LoadReviews()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5133/api/review");
            {
                var json = await response.Content.ReadAsStringAsync();
                var reviews = JsonSerializer.Deserialize<List<Review>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Reviews.Clear();
                foreach (var review in reviews)
                    Reviews.Add(review);
            }
        }

        private async Task DeleteReview()
        {
            if (SelectedReview == null) return;

            using var client = new HttpClient();
            var response = await client.DeleteAsync($"http://localhost:5133/api/review/{SelectedReview.id}");
            if (response.IsSuccessStatusCode)
            {
                Reviews.Remove(SelectedReview);
                SelectedReview = null;
            }
            else
            {
                MessageBox.Show("Nem sikerült törölni az értékelést.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
