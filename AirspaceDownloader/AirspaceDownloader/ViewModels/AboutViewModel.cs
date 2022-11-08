using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AirspaceDownloader.Models;
using AirspaceDownloader.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AirspaceDownloader.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IFileDownloader _fileDownloader = DependencyService.Get<IFileDownloader>();
        private bool _isDownloadEnabled = true;

        public AboutViewModel()
        {
            Title = "Airspace Downloader";
            DownloadFileCommand = new Command(OnDownloadFileClick);
            IsDownloadEnabled = true;

            _fileDownloader.OnFileDownloaded += OnFileDownloaded;
        }

        public ICommand DownloadFileCommand { get; }

        public bool IsDownloadEnabled
        {
            get => _isDownloadEnabled;
            set
            {
                SetProperty(ref _isDownloadEnabled, value);
                OnPropertyChanged(nameof(IsDownloadEnabled));
            }
        }

        private async Task RequestPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            var status2 = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status != PermissionStatus.Granted) await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status2 != PermissionStatus.Granted) await Permissions.RequestAsync<Permissions.StorageRead>();
        }

        /// <summary>
        ///     OnDownloadFileClick
        /// </summary>
        /// <param name="obj"></param>
        private async void OnDownloadFileClick(object obj)
        {
            await Console.Out.WriteLineAsync("ok");
            IsDownloadEnabled = false;
            await RequestPermissions();

            _fileDownloader.DownloadFile("https://planeur-net.github.io/airspace/france.txt", "");
        }

        /// <summary>
        ///     OnFileDownloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                MessagingCenter.Send(this, FileDownloadResult.Success.ToString());
                Console.Out.WriteLine("XF Downloader: File Saved Successfully", "File Saved Successfully", "Close");
            }

            else
            {
                MessagingCenter.Send(this, FileDownloadResult.Error.ToString(), e.ErrorMessage);
                Console.Out.WriteLine("XF Downloader: Error while saving the file", "Error while saving the file",
                    "Close");
            }

            IsDownloadEnabled = true;
        }
    }
}