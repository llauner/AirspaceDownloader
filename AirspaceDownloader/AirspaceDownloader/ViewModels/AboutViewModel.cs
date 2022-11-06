using AirspaceDownloader.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AirspaceDownloader.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IDownloader downloader = DependencyService.Get<IDownloader>();

        public AboutViewModel()
        {
            Title = "About";
            DownloadFileCommand = new Command(OnDownloadFile);

            downloader.OnFileDownloaded += OnFileDownloaded;
        }

        public ICommand DownloadFileCommand { get; }

        private async Task RequestPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            var status2 = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status != PermissionStatus.Granted) await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status2 != PermissionStatus.Granted) await Permissions.RequestAsync<Permissions.StorageRead>();
        }


        private async void OnDownloadFile(object obj)
        {
            await Console.Out.WriteLineAsync("ok");
            await RequestPermissions();

            downloader.DownloadFile("https://planeur-net.github.io/airspace/france.txt", "XF_Downloads");
        }

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
                Console.Out.WriteLine("XF Downloader: File Saved Successfully", "File Saved Successfully", "Close");
            else
                Console.Out.WriteLine("XF Downloader: Error while saving the file", "Error while saving the file", "Close");
        }

    }
}