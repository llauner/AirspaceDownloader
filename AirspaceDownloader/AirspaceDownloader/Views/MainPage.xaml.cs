using AirspaceDownloader.Models;
using AirspaceDownloader.ViewModels;
using Xamarin.Forms;

namespace AirspaceDownloader.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<MainViewModel>(this, FileDownloadResult.Success.ToString(),
                async sender => { await DisplayAlert("Alert", "File downloaded !", "OK"); });

            MessagingCenter.Subscribe<MainViewModel, string>(this, FileDownloadResult.Error.ToString(),
                async (sender, errorMessage) =>
                {
                    await DisplayAlert("Error downloading file !", errorMessage, "OK");
                });
        }

        protected override void OnDisappearing()
        {
        }
    }
}