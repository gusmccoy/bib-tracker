using bib_tracker.Shared;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace bib_tracker.Pages
{
    public sealed partial class RaceSettings : Page
    {
        public RaceSettings()
        {
            this.InitializeComponent();
            NameInput.Text = SharedData.RACE_TITLE;
            StartTimeDatePicker.Date = SharedData.RACE_START_TIME;
            StartTimePicker.Time = SharedData.RACE_START_TIME.TimeOfDay;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SharedData.RACE_TITLE = NameInput.Text.Trim();

            string datetime = StartTimeDatePicker.Date.ToString().Substring(0,10) + ' ' + StartTimePicker.Time.ToString();
            SharedData.RACE_START_TIME = DateTime.Parse(datetime);
        }
    }
}
