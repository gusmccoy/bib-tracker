using bib_tracker.Dialog;
using bib_tracker.Services;
using bib_tracker.Shared;
using bib_tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int stationId;

        public CheckInRunners()
        {
            this.InitializeComponent();
            ParticipantCheckInService = new ParticipantCheckInService();
            ParticipantService = new ParticipantService();
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
                ParticipantCheckInService.Add(new CheckInViewModel(stationId, bib, DateTime.Now));
                CheckIns.Add(new CheckInViewModel(stationId, bib, DateTime.Now));

                ParticipantViewModel participant = ParticipantService.GetParticipantByBibNumber(bib);
                if(RemainingParticipants.Contains(participant))
                {
                    RemainingParticipants.Remove(participant);
                }

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
                this.MainTextBlock.Text = "Exporting data to " + file.Path;
                ParticipantCheckInService.WriteCurrentData(file);
            }
            else
            {
                this.MainTextBlock.Text = "Operation cancelled.";
            }
        }
    }
}
