using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtazasSzervezo_Admin.Commands;
using UtazasSzervezo_Admin.Views;

namespace UtazasSzervezo_Admin.ViewModels
{
    public class MainViewModel : BaseViewModel 
    {
        private object _currentView ;
        public object CurrentView 
        {
            get => _currentView ;
            set => SetProperty(ref _currentView , value);
        }

        public ICommand ShowAccommodationCommand { get; }
        public ICommand ShowFlightCommand { get; }
        public ICommand ShowReviewCommand { get; }
        public ICommand ShowStatisticsCommand { get; }

        public MainViewModel ()
        {
            ShowAccommodationCommand = new RelayCommand(_ => CurrentView  = new AccommodationView());
            ShowFlightCommand = new RelayCommand(_ => CurrentView  = new FlightView());
            ShowReviewCommand = new RelayCommand(_ => CurrentView  = new ReviewListView());
            ShowStatisticsCommand = new RelayCommand(_ => CurrentView  = new StatisticsView());

            // Kezdőnézet
            CurrentView  = new AccommodationView();
        }
    }
}
