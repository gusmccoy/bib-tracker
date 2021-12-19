using bib_tracker.Model;
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

namespace bib_tracker.Pages
{

    public sealed partial class CheckInRunners : Page
    {
        public ObservableCollection<CheckInViewModel> CheckIns = new ObservableCollection<CheckInViewModel>();
        public ParticipantCheckInService ParticipantCheckInService;
        private string StationName = "station1";
        public CheckInRunners()
        {
            this.InitializeComponent();
            ParticipantCheckInService = new ParticipantCheckInService();
            this.PopulateExistingCheckInRecordsByStationName();
        }

        private void PopulateExistingCheckInRecordsByStationName()
        {
            var checkIns = ParticipantCheckInService.GetAllParticipantCheckInsByStation(1);

            if (checkIns.Count != 0)
            {
                foreach (var checkIn in checkIns)
                {
                    this.CheckIns.Add(checkIn);
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
                ParticipantCheckInService.Add(new CheckInViewModel(1, bib, DateTime.Now));
                CheckIns.Add(new CheckInViewModel(1, bib, DateTime.Now));
                this.BibInput.Text = "";
            }
        }
    }
}
