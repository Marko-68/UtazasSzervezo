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
using System.Windows.Shapes;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Admin.Views
{
    /// <summary>
    /// Interaction logic for FlightEditorView.xaml
    /// </summary>
    public partial class FlightEditorView : Window
    {
        public Flight EditedFlight { get; private set; }

        public FlightEditorView(Flight flight = null)
        {
            InitializeComponent();

            EditedFlight = flight != null ? new Flight
            {
                id = flight.id,
                airline = flight.airline,
                departure_time = flight.departure_time,
                arrival_time = flight.arrival_time,
                departure_airport = flight.departure_airport,
                destination_airport = flight.destination_airport
            } : new Flight
            {
                departure_time = DateTime.Now,
                arrival_time = DateTime.Now.AddHours(2)
            };

            DataContext = EditedFlight;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
