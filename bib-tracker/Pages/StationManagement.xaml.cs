using bib_tracker.Services;
using bib_tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace bib_tracker
{

    public sealed partial class StationManagement : Page
    {
        public ObservableCollection<StationViewModel> Stations = new ObservableCollection<StationViewModel>();
        public StationService StationService;
        public StationManagement()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            StationService = new StationService();
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

        private async void ImportStationsBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                StationService.ReadStationFile(file);
            }
        }

        private async void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "ExportedStationData";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                StationService.WriteCurrentData(file);
            }
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            var stations = StationService.GetAllStations();
            foreach (StationViewModel station in stations)
            {
                Stations.Add(station);
            }
        }
    }
}
