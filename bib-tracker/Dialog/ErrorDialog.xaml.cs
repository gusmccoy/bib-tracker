using Windows.UI.Xaml.Controls;

namespace bib_tracker.Dialog
{
    public sealed partial class ErrorDialog : ContentDialog
    {

        public ErrorDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
