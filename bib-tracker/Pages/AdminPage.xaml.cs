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
    }
}
