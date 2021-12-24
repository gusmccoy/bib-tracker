using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using bib_tracker.Model;
using System.Collections.ObjectModel;
using bib_tracker.Pages;
using bib_tracker.Shared;

namespace bib_tracker
{
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Participant> Participants;
        DispatcherTimer Timer = new DispatcherTimer();
        DispatcherTimer RaceTimer = new DispatcherTimer();
        DateTimeOffset startTime = DateTime.Parse("12/24/2021 09:16:00 AM");
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = this;
            if (SharedData.RACE_TITLE.Trim() != "")
                RaceName.Text = SharedData.RACE_TITLE.Trim();
            else
                RaceName.Text = "Set Race Name in Admin Page";
            if (SharedData.RACE_START_TIME != DateTime.Parse("01/01/0001 01:00:00 AM"))
            {
                RaceTimer.Tick += RaceTime_Tick;
                RaceTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                RaceTimer.Start();
            }
            else
            {
                RaceTime.Text = "Set Race Start Time in Admin Page";
            }
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void CheckInRunnersBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CheckInRunners));
        }

        private void AdminPageBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdminPage));
        }

        private void Timer_Tick(object sender, object e)
        {
            Time.Text = DateTime.Now.ToString("h:mm:ss tt");
        }

        private void RaceTime_Tick(object sender, object e)
        {
            TimeSpan duration = DateTime.Now - startTime;
            RaceTime.Text = GetRaceTime(duration);
        }

        private string GetRaceTime(TimeSpan duration)
        {
            string time = "";
            if ((int)duration.TotalHours < 10)
                time += '0';
            time += ((int)duration.TotalHours).ToString() + ':';
           if ((int)duration.TotalMinutes % 60 < 10)
                time += '0'; 
            time += ((int)(duration.TotalMinutes % 60)).ToString() + ':';
            if (((int)(duration.TotalSeconds % 3600) % 60) < 10)
                time += '0';
            time += ((int)(duration.TotalSeconds % 3600) % 60).ToString() + '.';
            if (((int)((duration.TotalMilliseconds % 3600) % 60) % 100) < 10)
                time += '0';
            time += ((int)((duration.TotalMilliseconds % 3600) % 60) % 100).ToString();
            return time;
        }
    }
}
