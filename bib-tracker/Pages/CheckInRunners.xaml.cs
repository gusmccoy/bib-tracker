using bib_tracker.Dialog;
using bib_tracker.Services;
using bib_tracker.Shared;
using bib_tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace bib_tracker.Pages
{

    public sealed partial class CheckInRunners : Page
    {
        public ObservableCollection<CheckInViewModel> CheckIns = new ObservableCollection<CheckInViewModel>();
        public ObservableCollection<ParticipantViewModel> RemainingParticipants = new ObservableCollection<ParticipantViewModel>();
        public ParticipantCheckInService ParticipantCheckInService;
        public ParticipantService ParticipantService;
        public StationService StationService;
        private int stationId;

        public CheckInRunners()
        {
            this.InitializeComponent();
            ParticipantCheckInService = new ParticipantCheckInService();
            ParticipantService = new ParticipantService();
            StationService = new StationService();
            GetStationInfo();
        }

        private async void GetStationInfo()
        {
            StationLogin login = new StationLogin();
            await login.ShowAsync();

            if(login.Result == SignInResult.SignInOK)
            {
                stationId = SharedData.STATION_ID;
                PopulateExistingCheckInRecordsByStationName();
                if(CheckIns.Count != 0)
                    PopulateRemainingParticipants();
            }
            else if(login.Result == SignInResult.SignInCancel)
            {
                Frame.Navigate(typeof(MainPage));
            }
        }

        private void PopulateExistingCheckInRecordsByStationName()
        {
            CheckIns.Clear();
            if(StationService.GetStationById(stationId).Number == 0)
            {
                StationService.AddStation(stationId);
            }
            var checkIns = ParticipantCheckInService.GetAllParticipantCheckInsByStation(stationId);

            if (checkIns.Count != 0)
            {
                foreach (var checkIn in checkIns)
                {
                    this.CheckIns.Add(checkIn);
                }
            }
        }

        private void PopulateRemainingParticipants()
        {
            RemainingParticipants.Clear();
            var participants = ParticipantCheckInService.GetAllRemainingParticipantsByStationId(stationId);

            if (participants.Count != 0)
            {
                foreach (var participant in participants)
                {
                    this.RemainingParticipants.Add(participant);
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox textBox = sender as TextBox;
                string input = textBox.Text.Trim();
                int bib = Int32.Parse(input);
                if(ParticipantService.GetParticipantByBibNumber(bib).Bib == 0)
                {
                    ParticipantService.AddParticipant(new ParticipantViewModel()
                    {
                        Bib = bib,
                        FirstName = "NA",
                        LastName = "NA"
                    });
                }
                ParticipantCheckInService.Add(new CheckInViewModel(stationId, bib, DateTime.Now));
                CheckIns.Add(new CheckInViewModel(stationId, bib, DateTime.Now));
                RemainingParticipants.Clear();
                PopulateRemainingParticipants();
                this.BibInput.Text = "";
            }
        }

        private async void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "ExportedStationCheckInData";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                ParticipantCheckInService.WriteCurrentData(file);
            }
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            CheckIns.Clear();
            RemainingParticipants.Clear();
            var checkins = ParticipantCheckInService.GetAllParticipantCheckIns();
            foreach (CheckInViewModel checkin in checkins)
            {
                CheckIns.Add(checkin);
            }

            foreach(ParticipantViewModel participant in ParticipantCheckInService.GetAllRemainingParticipantsByStationId(stationId))
            {
                RemainingParticipants.Add(participant);
            }
        }
    }
}
