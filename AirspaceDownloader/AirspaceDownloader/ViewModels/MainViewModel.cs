﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public static string AirspaceFileUrl { get; } = "https://planeur-net.github.io/airspace/france.txt";
        public static string AirspaceOpenAirStandardFileUrl { get; } = "https://planeur-net.github.io/airspace/france_openair_standard.txt";
        public static string GuideDesAiresFileUrl { get; } = "https://planeur-net.github.io/outlanding/guide_aires_securite.cup";
        public static string ChampsDesAlpesFileUrl { get; } = "https://planeur-net.github.io/outlanding/champs_des_alpes.cup";
        public static string ColsDesAlpesFileUrl { get; } = "https://planeur-net.github.io/outlanding/cols_des_alpes.cup";
        public static string MountainPeaksFR { get; } = "https://planeur-net.github.io/outlanding/mountain_peaks_FR.cup";
        public static string MountainPeaksCH { get; } = "https://planeur-net.github.io/outlanding/mountain_peaks_CH.cup";
        public static string MountainPeaksIT { get; } = "https://planeur-net.github.io/outlanding/mountain_peaks_IT.cup";
        public static string XCsoarCombinedFileURL { get; } = "https://planeur-net.github.io/outlanding/combined_guide+champs.xcsoar.zip";
        public static string LudoWaypointsFileUrl { get; } = "https://planeur-net.github.io/outlanding/Misc/Ludo_waypoints.cup";

        public List<FileDescription> ListFilesToDownload = new List<FileDescription> {
            new FileDescription(XCsoarCombinedFileURL, true, true),
            new FileDescription(AirspaceFileUrl, false),
            new FileDescription(AirspaceOpenAirStandardFileUrl, false),
            new FileDescription(GuideDesAiresFileUrl, false),
            new FileDescription(ChampsDesAlpesFileUrl, false),
            new FileDescription(ColsDesAlpesFileUrl, false),
            new FileDescription(MountainPeaksFR, false),
            new FileDescription(MountainPeaksCH, false),
            new FileDescription(MountainPeaksIT, false),
            new FileDescription(LudoWaypointsFileUrl, false)
        };

        public ICommand DownloadFileCommand { get; }
        public ICommand ResetXCSoarDownloadPathCommand { get; }

        private readonly IFileDownloader _fileDownloader = DependencyService.Get<IFileDownloader>();
        private bool _isDownloadEnabled = true;
        private bool _isTargetSelectedDownloads = true;
        private bool _isTargetSelectedXcSoar = true;
        private string _seeYouDownloadPath = null;
        private string _seeYouAirsapceDownloadPath = null;
        private string _seeYouBothFilesDownloadPath = null;
        private string _xcsoarDownloadPath = null;
        private string _logText = "";

        /// <summary>
        /// </summary>
        public MainViewModel()
        {
            Title = "github.com Downloader";
            DownloadFileCommand = new Command(OnDownloadFileClick);
            ResetXCSoarDownloadPathCommand = new Command(ResetXCSoarDownloadPathClick);
            IsDownloadEnabled = true;

            _fileDownloader.OnFileDownloaded += OnFileDownloaded;
            _fileDownloader.OnLogUpdateRequested += OnLogUpdateRequested;

            var prefSeeYouDownloadPath = Preferences.Get("seeYouStorePath", null);
            SeeYouDownloadPath = (prefSeeYouDownloadPath is null) ? _fileDownloader.GetDefaultSeeYouDownloadPath() : prefSeeYouDownloadPath;
            _fileDownloader.SeeYouDownloadPath = SeeYouDownloadPath;

            var prefSeeYouAirspaceDownloadPath = Preferences.Get("seeYouAirsapceStorePath", null);
            SeeYouAirspaceDownloadPath = (prefSeeYouAirspaceDownloadPath is null) ? _fileDownloader.GetDefaultSeeYouDownloadPath() : prefSeeYouAirspaceDownloadPath;
            _fileDownloader.SeeYouAirspaceDownloadPath = SeeYouAirspaceDownloadPath;

            var prefSeeYouBothFilesDownloadPath = Preferences.Get("seeYouBothFilesDownloadPath", null);
            SeeYouBothFilesDownloadPath = (prefSeeYouBothFilesDownloadPath is null) ? _fileDownloader.GetDefaultSeeYouDownloadPath() : prefSeeYouBothFilesDownloadPath;
            _fileDownloader.SeeYouBothFilesDownloadPath = SeeYouBothFilesDownloadPath;


            var prefXCSoarDownloadPath = Preferences.Get("xcsoarStorePath", null);
            XCSoarDownloadPath = (prefXCSoarDownloadPath is null) ? _fileDownloader.GetDfaultXCSoarDownloadPath() : prefXCSoarDownloadPath;
            _fileDownloader.XCSoarDownloadPath = XCSoarDownloadPath;

            IsTargetSelectedDownloads = Preferences.Get("IsTargetSelectedDownloads", true);
            IsTargetSelectedXcSoar = Preferences.Get("IsTargetSelectedXcSoar", true);
        }

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
                Preferences.Set("IsTargetSelectedDownloads", value);

                SetProperty(ref _isTargetSelectedDownloads, value);
                OnPropertyChanged(nameof(IsTargetSelectedDownloads));
            }
        }

        public bool IsTargetSelectedXcSoar
        {
            get => _isTargetSelectedXcSoar;
            set
            {
                Preferences.Set("IsTargetSelectedXcSoar", value);

                SetProperty(ref _isTargetSelectedXcSoar, value);
                OnPropertyChanged(nameof(IsTargetSelectedXcSoar));
            }
        }

        public string SeeYouDownloadPath
        {
            get => _seeYouDownloadPath;
            set
            {
                value = string.IsNullOrEmpty(value) ? null : value;
                Preferences.Set("seeYouStorePath", value);

                SetProperty(ref _seeYouDownloadPath, value);
                OnPropertyChanged(nameof(SeeYouDownloadPath));
            }
        }

        public string SeeYouAirspaceDownloadPath
        {
            get => _seeYouAirsapceDownloadPath;
            set
            {
                value = string.IsNullOrEmpty(value) ? null : value;
                Preferences.Set("seeYouAirsapceStorePath", value);

                SetProperty(ref _seeYouAirsapceDownloadPath, value);
                OnPropertyChanged(nameof(SeeYouAirspaceDownloadPath));
            }
        }

        public string SeeYouBothFilesDownloadPath
        {
            get => _seeYouBothFilesDownloadPath;
            set
            {
                value = string.IsNullOrEmpty(value) ? null : value;
                Preferences.Set("seeYouBothFilesDownloadPath", value);

                SetProperty(ref _seeYouBothFilesDownloadPath, value);
                OnPropertyChanged(nameof(SeeYouBothFilesDownloadPath));
            }
        }

        public string XCSoarDownloadPath
        {
            get => _xcsoarDownloadPath;
            set
            {
                value = string.IsNullOrEmpty(value) ? null : value;
                Preferences.Set("xcsoarStorePath", value);

                SetProperty(ref _xcsoarDownloadPath, value);
                OnPropertyChanged(nameof(XCSoarDownloadPath));
            }
        }

        public string LogText
        {
            get => _logText;
            set
            {
                value = string.IsNullOrEmpty(value) ? null : value;

                SetProperty(ref _logText, value);
                OnPropertyChanged(nameof(LogText));
            }
        }

        /// <summary>
        ///     OnDownloadFileClick
        /// </summary>
        /// <param name="obj"></param>
        private async void OnDownloadFileClick(object obj)
        {
            Console.Out.WriteLine($"IsTargetSelectedDownloads={IsTargetSelectedDownloads}");
            Console.Out.WriteLine($"IsTargetSelectedXcSoar={IsTargetSelectedXcSoar}");

            // Store path preferences
            //Preferences.Set("xcsoarStorePath", XCSoarDownloadPath);

            // Setup fileDownloader Parameters
            _fileDownloader.SeeYouDownloadPath = SeeYouDownloadPath;
            _fileDownloader.SeeYouAirspaceDownloadPath = SeeYouAirspaceDownloadPath;
            _fileDownloader.XCSoarDownloadPath = XCSoarDownloadPath;
            _fileDownloader.IsSaveForDownloads = IsTargetSelectedDownloads;
            _fileDownloader.IsSaveForXcSoar = IsTargetSelectedXcSoar;

            // Get Download locations
            IsDownloadEnabled = false;
            await RequestPermissions();

            // ### Purge List of files to download
            // Remove files that will not be used
            foreach (var desc in ListFilesToDownload.ToList())
            {
                if (!IsTargetSelectedXcSoar && desc.IsXCSoarOnly)
                {
                    ListFilesToDownload.Remove(desc);
                }
            }
            // Download files
            _fileDownloader.DownloadFiles(ListFilesToDownload);
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
                if (_fileDownloader.IsDownloadBatchFinished)
                {
                    MessagingCenter.Send(this, FileDownloadResult.Success.ToString());
                    Console.Out.WriteLine("File Saved Successfully", "File Saved Successfully", "Close");
                }
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

        private void OnLogUpdateRequested(object sender, string logText)
        {
            logText += "\r\n";
            LogText += logText;
        }

        private void ResetXCSoarDownloadPathClick(object obj)
        {
            XCSoarDownloadPath = _fileDownloader.GetDfaultXCSoarDownloadPath();
        }
    }
}