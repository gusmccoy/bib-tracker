using bib_tracker.Model;
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
using bib_tracker.ViewModel;
using bib_tracker.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace bib_tracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ParticipantManagement : Page
    {
        public ObservableCollection<ParticipantViewModel> Participants;
        public ParticipantService ParticipantService;
        public ParticipantManagement()
        {
            this.InitializeComponent();
            ParticipantService = new ParticipantService();
            Participants = new ObservableCollection<ParticipantViewModel>();
            PopulateExistingParticipantRecords();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void PopulateExistingParticipantRecords()
        {
            var participants = ParticipantService.GetAllParticipants();

            if(participants.Count != 0)
            {
                foreach (var participant in participants)
                {
                    this.Participants.Add(participant);
                }
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
                this.MainTextBlock.Text = "Reading in " + file.Path;
                ParticipantService.LoadFile(file);
                Participants.Clear();

                foreach (ParticipantViewModel runner in ParticipantService.GetAllParticipants())
                {
                    Participants.Add(runner);
                }
            }
            else
            {
                this.MainTextBlock.Text = "Operation cancelled.";
            }
        }
    }
}
