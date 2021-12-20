using bib_tracker.Services;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

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
                this.MainTextBlock.Text = "Reading in " + file.Path;
                StationService.ReadStationFile(file);
            }
            else
            {
                this.MainTextBlock.Text = "Operation cancelled.";
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
            {
                this.MainTextBlock.Text = "Exporting data to " + file.Path;
                StationService.WriteCurrentData(file);
            }
            else
            {
                this.MainTextBlock.Text = "Operation cancelled.";
            }
        }
    }
}
