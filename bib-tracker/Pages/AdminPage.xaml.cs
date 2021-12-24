using bib_tracker.Services;
using bib_tracker.Shared;
using System;
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
            SharedData.RACE_START_TIME = DateTime.Parse("01/01/0001 01:00:00 AM");

            var participants = ParticipantService.GetAllParticipants();
            var stations = StationService.GetAllStations();

            foreach(var runner in participants)
            {
                ParticipantService.DeleteParticipant(runner.Bib);
            }

            foreach(var station in stations)
            {
                StationService.DeleteStation(station.Number);
                ParticipantCheckInService.DeleteAllRecordsByStationNumber(station.Number);
            }
        }
    }
}
