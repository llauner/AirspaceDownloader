using System;
using AirspaceDownloader.Models;
using AirspaceDownloader.Services;
using AirspaceDownloader.ViewModels;
using Xamarin.Forms;

namespace AirspaceDownloader.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<AboutViewModel>(this, FileDownloadResult.Success.ToString(),  async (sender) =>
            {
                await DisplayAlert("Alert", "File downloaded !", "OK");
            });

            MessagingCenter.Subscribe<AboutViewModel, string>(this, FileDownloadResult.Error.ToString(), async (sender, errorMessage) =>
            {
                await DisplayAlert("Error downloading file !", errorMessage, "OK");
            });
        }

        protected override void OnDisappearing()
        {

        }

    }
}