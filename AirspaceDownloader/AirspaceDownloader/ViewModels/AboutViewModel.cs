using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AirspaceDownloader.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            DownloadFileCommand = new Command(OnDownloadFile);
        }

        public ICommand DownloadFileCommand { get; }


        private async void OnDownloadFile(object obj)
        {
            await Console.Out.WriteLineAsync("ok");
        }
    }
}