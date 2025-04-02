using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Admin.Services;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Admin.ViewModels
{
    public class AccommodationViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private ObservableCollection<Accommodation> _accommodations;

        public ObservableCollection<Accommodation> Accommodations
        {
            get => _accommodations;
            set => SetProperty(ref _accommodations, value);
        }

        public AccommodationViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Accommodations = new ObservableCollection<Accommodation>();
            LoadAccommodations();
        }

        private async void LoadAccommodations()
        {
            var accommodations = await _apiService.GetAsync<List<Accommodation>>("api/accommodation");
            foreach (var accommodation in accommodations)
            {
                Accommodations.Add(accommodation);
            }
        }
    }
}
