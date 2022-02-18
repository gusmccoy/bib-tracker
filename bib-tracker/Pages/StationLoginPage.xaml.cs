using bib_tracker.Services;
using bib_tracker.Shared;
using bib_tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace bib_tracker.Pages
{

    public sealed partial class StationLoginPage : Page
    {
        public ObservableCollection<StationViewModel> Stations = new ObservableCollection<StationViewModel>();
        public StationService StationService;
        public StationLoginPage()
        {
            this.InitializeComponent();
            StationService = new StationService();
            Stations = new ObservableCollection<StationViewModel>();
            Stations.Clear();
            PopulateExistingStationRecords();
        }

        private void PopulateExistingStationRecords()
        {
            var stations = StationService.GetAllStations();

            if (stations.Count != 0)
            {
                foreach (var station in stations)
                {
                    this.Stations.Add(station);
                }
            }
        }

        private void LoginInButton_Click(object sender, RoutedEventArgs e)
        {
            string input = LoginTextBox.Text.Trim();
            try
            {
                int stationId = Int32.Parse(input);
                SharedData.STATION_ID = stationId;
                this.Frame.Navigate(typeof(CheckInRunners));
            }
            catch (Exception)
            {
                LoginTextBox.Text = "";
            }
        }
    }
}
