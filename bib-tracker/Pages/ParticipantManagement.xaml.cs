using bib_tracker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using bib_tracker.ViewModel;
using bib_tracker.Services;


namespace bib_tracker
{
    public sealed partial class ParticipantManagement : Page
    {
        public ObservableCollection<ParticipantViewModel> Participants;
        public ParticipantService ParticipantService;
        public ParticipantManagement()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Participants = new ObservableCollection<ParticipantViewModel>();
            ParticipantService = new ParticipantService();
            PopulateExistingParticipantRecords();
        }

        private void PopulateExistingParticipantRecords()
        {
            var participants = ParticipantService.GetAllParticipants();

            foreach (var participant in participants)
            {
                Participants.Add(participant);
            }
        }

        private async void ImportParticipantsBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                ParticipantService.LoadFile(file);
                Participants.Clear();
            }
        }

        private async void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "ExportedParticipantData";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
                ParticipantService.WriteCurrentData(file);
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            var runners = ParticipantService.GetAllParticipants();
            foreach (ParticipantViewModel runner in runners)
            {
                Participants.Add(runner);
            }
        }
    }
}
