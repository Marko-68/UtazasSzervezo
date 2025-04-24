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
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Admin.Views
{
    /// <summary>
    /// Interaction logic for AccommodationEditorView.xaml
    /// </summary>
    public partial class AccommodationEditorView : Window
    {
        public Accommodation EditedAccommodation { get; private set; }

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
                address = acc.address,
                city = acc.city,
                country = acc.country
            } : new Accommodation();

            DataContext = EditedAccommodation;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
