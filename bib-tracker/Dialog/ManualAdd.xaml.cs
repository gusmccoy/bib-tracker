﻿using bib_tracker.Services;
using bib_tracker.Shared;
using System;
using Windows.UI.Xaml.Controls;

namespace bib_tracker.Dialog
{

    public sealed partial class ManualAdd : ContentDialog
    {

        public SignInResult Result { get; private set; }
        ParticipantCheckInService checkInService;

        public ManualAdd()
        {
            this.InitializeComponent();
            checkInService = new ParticipantCheckInService();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string input = LoginTextBox.Text.Trim();
            try
            {
                int stationId = Int32.Parse(input);
                SharedData.STATION_ID = stationId;
            }
            catch (Exception e)
            {
                LoginTextBox.Text = "";
            }

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = SignInResult.SignInCancel;
        }
    }
}
