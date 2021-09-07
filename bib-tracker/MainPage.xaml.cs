using System;
using System.Collections.Generic;
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
using bib_tracker.DataAccess;
using bib_tracker.Model;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace bib_tracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Participant> Participants;
        public MainPage()
        {
            this.InitializeComponent();
            Participants = new ObservableCollection<Participant>();
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
                this.MainTextBlock.Text = "Reading in " + file.Path;
                SqliteDb.ReadFileData("PARTICIPANT", file);

                foreach (Participant runner in SqliteDb.GetAllParticipants())
                    Participants.Add(runner);

            }
            else
            {
                this.MainTextBlock.Text = "Operation cancelled.";
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
                SqliteDb.ReadFileData("STATION", file);
            }
            else
            {
                this.MainTextBlock.Text = "Operation cancelled.";
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
                this.MainTextBlock.Text = "Reading in " + file.Path;
                SqliteDb.ReadFileData("CHECKIN", file);
            }
            else
            {
                this.MainTextBlock.Text = "Operation cancelled.";
            }
        }
    }
}
