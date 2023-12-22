﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using AirspaceDownloader.Droid;
using AirspaceDownloader.Services;
using Xamarin.Forms;
using Environment = Android.OS.Environment;

[assembly: Dependency(typeof(AndroidFileDownloader))]

namespace AirspaceDownloader.Droid
{
    public class AndroidFileDownloader : IFileDownloader
    {
        private readonly string _downloadPath =
            Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath;

        private readonly string _xcsoarPath =
            Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, "XCSoarData");

        private List<string> _listUrlsToDownload = new List<string>();
        private int _filesDownloadedCount = 0;

        public bool IsSaveForDownloads { get; set; }
        public bool IsSaveForXcSoar { get; set; }
        public int NbFilesToDownload => _listUrlsToDownload.Count;
        public int FilesDownloadedCount
        {
            get
            {
                return _filesDownloadedCount;
            }
            set 
            { 
                _filesDownloadedCount= value;
            }
        }
        public bool IsDownloadBatchFinished => NbFilesToDownload == FilesDownloadedCount;

        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public AndroidFileDownloader()
        {
            FilesDownloadedCount = 0;
        }

        /// <summary>
        /// Download Outlanding, mountain passes, waypoints from repo: https://github.com/planeur-net/outlanding
        /// </summary>
        /// <param name="listUrls"></param>
        public void DownloadFiles(List<string> listUrls)
        {
            FilesDownloadedCount = 0;
            _listUrlsToDownload = listUrls;
            foreach (var url in _listUrlsToDownload)
            {
                try
                {
                    var downloadedFileName = Path.GetFileName(url);
                    // Download
                    var webClient = new WebClient();
                    webClient.DownloadDataCompleted += DownloadDataCallback;

                    webClient.DownloadDataAsync(new Uri(url), downloadedFileName);
                }
                catch (Exception ex)
                {
                    if (OnFileDownloaded != null)
                        OnFileDownloaded.Invoke(this, new DownloadEventArgs(false, ex.Message));
                }
            }
        }


        /// <summary>
        ///     DownloadDataCallback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadDataCallback(object sender, DownloadDataCompletedEventArgs e)
        {
            // Success: the file was downloaded successfully
            if (!e.Cancelled && e.Error == null)
            {
                // Get the file
                var data = e.Result;
                var textData = Encoding.UTF8.GetString(data);
                var downloadedFileName = e.UserState.ToString();

                // Save to disk
                if (IsSaveForDownloads) // Downloads: SeeYou Navigator
                {
                    var pathToNewFile = Path.Combine(_downloadPath, downloadedFileName);
                    File.WriteAllText(pathToNewFile, textData);
                }

                if (IsSaveForXcSoar) // XCSoar
                {
                    var pathToNewFile = Path.Combine(_xcsoarPath, downloadedFileName);
                    File.WriteAllText(pathToNewFile, textData);
                }

                // Notify: 
                FilesDownloadedCount++;
                OnFileDownloaded?.Invoke(this, new DownloadEventArgs(true));
            }
            else
            {
                // Error while downloading the file
                OnFileDownloaded?.Invoke(this, new DownloadEventArgs(false, e.Error.ToString()));
            }
        }
    }
}