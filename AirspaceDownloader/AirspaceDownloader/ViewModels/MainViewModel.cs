using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AirspaceDownloader.Models;
using AirspaceDownloader.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AirspaceDownloader.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IFileDownloader _fileDownloader = DependencyService.Get<IFileDownloader>();
        private bool _isDownloadEnabled = true;
        private bool _isTargetSelectedDownloads = true;
        private bool _isTargetSelectedXcSoar = true;

        /// <summary>
        /// </summary>
        public MainViewModel()
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

        public bool IsTargetSelectedDownloads
        {
            get => _isTargetSelectedDownloads;
            set
            {
                SetProperty(ref _isTargetSelectedDownloads, value);
                OnPropertyChanged(nameof(IsTargetSelectedDownloads));
            }
        }

        public bool IsTargetSelectedXcSoar
        {
            get => _isTargetSelectedXcSoar;
            set
            {
                SetProperty(ref _isTargetSelectedXcSoar, value);
                OnPropertyChanged(nameof(IsTargetSelectedXcSoar));
            }
        }

        public string AirspaceFileUrl { get; } = "https://planeur-net.github.io/airspace/france.txt";


        /// <summary>
        ///     OnDownloadFileClick
        /// </summary>
        /// <param name="obj"></param>
        private async void OnDownloadFileClick(object obj)
        {
            Console.Out.WriteLine($"IsTargetSelectedDownloads={IsTargetSelectedDownloads}");
            Console.Out.WriteLine($"IsTargetSelectedXcSoar={IsTargetSelectedXcSoar}");

            // Get Parameters
            _fileDownloader.IsSaveForDownloads = IsTargetSelectedDownloads;
            _fileDownloader.IsSaveForXcSoar = IsTargetSelectedXcSoar;

            // Get Download locations
            IsDownloadEnabled = false;
            await RequestPermissions();

            _fileDownloader.DownloadFile(AirspaceFileUrl);
        }


        /// <summary>
        ///     RequestPermissions
        /// </summary>
        /// <returns></returns>
        private async Task RequestPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            var status2 = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status != PermissionStatus.Granted) await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status2 != PermissionStatus.Granted) await Permissions.RequestAsync<Permissions.StorageRead>();
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
                // Success: Save File to designated locations
                if (IsTargetSelectedDownloads) // Save for SeeYouNavigator: /Downdloads
                {
                }

                if (IsTargetSelectedXcSoar) // Save for XCSoar :/XCSoarData
                {
                }

                // Notify
                MessagingCenter.Send(this, FileDownloadResult.Success.ToString());
                Console.Out.WriteLine("File Saved Successfully", "File Saved Successfully", "Close");
            }

            else
            {
                // Error while downloading the file
                // Notify
                MessagingCenter.Send(this, FileDownloadResult.Error.ToString(), e.ErrorMessage);
                Console.Out.WriteLine("Error while saving the file", "Error while saving the file", "Close");
            }

            IsDownloadEnabled = true;
        }
    }
}