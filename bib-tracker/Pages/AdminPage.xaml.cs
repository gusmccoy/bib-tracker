using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace bib_tracker.Pages
{
    public sealed partial class AdminPage : Page
    {
        public AdminPage()
        {
            this.InitializeComponent();
        }

        private void ParticipantManagementBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ParticipantManagement));
        }

        private void StationManagementBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StationManagement));
        }

        private void CheckInManagementBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CheckInManagement));
        }

        private void ManageRaceSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RaceSettings));
        }
    }
}
