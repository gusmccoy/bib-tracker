using bib_tracker.Services;
using bib_tracker.Shared;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace bib_tracker.Pages
{
    public sealed partial class AdminPage : Page
    {
        ParticipantService ParticipantService;
        StationService StationService;
        ParticipantCheckInService ParticipantCheckInService;

        public AdminPage()
        {
            this.InitializeComponent();
            ParticipantCheckInService = new ParticipantCheckInService();
            StationService = new StationService();
            ParticipantService = new ParticipantService();
        }

        private void ParticipantManagementBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ParticipantManagement));
        }

        private void StationManagementBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StationManagement));
        }

        private void CheckInManagementBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CheckInManagement));
        }

        private void ManageRaceSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RaceSettings));
        }

        private void ClearDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            SharedData.STATION_ID = 0;
            SharedData.RACE_TITLE = "";
            SharedData.RACE_START_TIME = DateTime.Parse("01/01/2022 01:00:00 AM");

            var participants = ParticipantService.GetAllParticipants();
            var stations = StationService.GetAllStations();

            foreach (var station in stations)
            {
                ParticipantCheckInService.DeleteAllRecordsByStationNumber(station.Number);
                StationService.DeleteStation(station.Number);
            }

            foreach (var runner in participants)
            {
                ParticipantService.DeleteParticipant(runner.Bib);
            }
        }
    }
}
