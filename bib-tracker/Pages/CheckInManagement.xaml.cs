using bib_tracker.Services;
using bib_tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace bib_tracker
{

    public sealed partial class CheckInManagement : Page
    {
        public ObservableCollection<CheckInViewModel> CheckIns = new ObservableCollection<CheckInViewModel>();
        public ParticipantCheckInService ParticipantCheckInService;
        public CheckInManagement()
        {
            this.InitializeComponent();
            ParticipantCheckInService = new ParticipantCheckInService();
            this.PopulateExistingCheckInRecords();
        }

        private void PopulateExistingCheckInRecords()
        {
            var checkIns = ParticipantCheckInService.GetAllParticipantCheckIns();

            if (checkIns.Count != 0)
            {
                foreach (var checkIn in checkIns)
                {
                    this.CheckIns.Add(checkIn);
                }
            }
        }

        private async void ImportCheckInsBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                ParticipantCheckInService.ReadCheckInFile(file);
            }
        }

        private async void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "ExportedAllCheckInData";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                ParticipantCheckInService.WriteCurrentData(file);
            }
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            CheckIns.Clear();
            var checkins = ParticipantCheckInService.GetAllParticipantCheckIns();
            foreach (CheckInViewModel checkin in checkins)
            {
                CheckIns.Add(checkin);
            }
        }
    }
}
