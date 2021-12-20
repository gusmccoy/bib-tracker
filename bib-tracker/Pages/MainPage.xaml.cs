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
using bib_tracker.Pages;


namespace bib_tracker
{
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Participant> Participants;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void CheckInRunnersBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CheckInRunners));
        }

        private void AdminPageBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdminPage));
        }
    }
}
