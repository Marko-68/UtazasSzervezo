using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Admin.Views
{
    /// <summary>
    /// Interaction logic for AccommodationEditorView.xaml
    /// </summary>
    public partial class AccommodationEditorView : Window
    {
        public Accommodation EditedAccommodation { get; private set; }
        public ObservableCollection<string> OptionsType= new ObservableCollection<string>
        {
            "Hotel",
            "Apartment",
            "Resort"
        };
        public ObservableCollection<string> OptionsDinning = new ObservableCollection<string>
        {
            "Breakfast",
            "Half-board",
            "All-inclusive"
        };

        public AccommodationEditorView(Accommodation acc = null)
        {
            InitializeComponent();
            
            EditedAccommodation = acc != null ? new Accommodation
            {
                id = acc.id,
                name = acc.name,
                description = acc.description,
                type = acc.type,
                number_of_rooms = acc.number_of_rooms,
                guests = acc.guests,
                address = acc.address,
                city = acc.city,
                country = acc.country,
                price_per_night = acc.price_per_night,
                star_rating = acc.star_rating,
                available_rooms = acc.available_rooms,
                dinning = acc.dinning,
                cover_img = acc.cover_img
            } : new Accommodation();

            DataContext = EditedAccommodation;
            cbtype.ItemsSource = OptionsType;
            cbdinning.ItemsSource = OptionsDinning;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
