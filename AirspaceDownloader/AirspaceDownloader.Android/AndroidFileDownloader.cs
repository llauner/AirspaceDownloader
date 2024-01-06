using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using AirspaceDownloader.Droid;
using AirspaceDownloader.Models;
using AirspaceDownloader.Services;
using Android.OS;
using Java.IO;
using Xamarin.Forms;
using static AndroidX.Navigation.ActivityNavigator;
using Environment = Android.OS.Environment;
using File = System.IO.File;

[assembly: Dependency(typeof(AndroidFileDownloader))]

namespace AirspaceDownloader.Droid
{
    public class AndroidFileDownloader : IFileDownloader
    {
        private readonly string _downloadPath =
            Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath;

        private string _xcsoarDownloadPath = null;
            

        private List<FileDescription> _listUrlsToDownload = new List<FileDescription>();
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

        public string XCSoarDownloadPath
        {
            get
            {
                return (_xcsoarDownloadPath is null) ? GetDfaultXCSoarDownloadPath() : _xcsoarDownloadPath;
            }
            set
            {
                value = (value is null) ? GetDfaultXCSoarDownloadPath() : value;
                _xcsoarDownloadPath = value;
            }
        }

        public bool IsDownloadBatchFinished => NbFilesToDownload == FilesDownloadedCount;

        public event EventHandler<DownloadEventArgs> OnFileDownloaded;
        public event EventHandler<string> OnLogUpdateRequested;

        public AndroidFileDownloader()
        {
            FilesDownloadedCount = 0;
        }

        /// <summary>
        /// Download Outlanding, mountain passes, waypoints from repo: https://github.com/planeur-net/outlanding
        /// </summary>
        /// <param name="listFileDescription"></param>
        public void DownloadFiles(List<FileDescription> listFileDescription)
        {
            FilesDownloadedCount = 0;
            _listUrlsToDownload = listFileDescription;
            foreach (var desc in _listUrlsToDownload)
            {
                try
                {
                    OnLogUpdateRequested?.Invoke(this, $"Downloading...: {desc.Url}");
                    // Download
                    var webClient = new WebClient();
                    webClient.DownloadDataCompleted += DownloadDataCallback;

                    webClient.DownloadDataAsync(new Uri(desc.Url), desc);
                }
                catch (Exception ex)
                {
                    if (OnFileDownloaded != null)
                        OnFileDownloaded.Invoke(this, new DownloadEventArgs(desc, false, ex.Message));
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
            var fileDescription = ((FileDescription)e.UserState);

            // Success: the file was downloaded successfully
            if (!e.Cancelled && e.Error == null)
            {
                // Get the file
                var data = e.Result;
                var textData = Encoding.UTF8.GetString(data);
                var downloadedFileName = fileDescription.Filename;

                // Log
                OnLogUpdateRequested?.Invoke(this, $"File Downloaded !: {downloadedFileName}");

                // Save to disk
                if (IsSaveForDownloads && !fileDescription.IsXCSoarOnly) // Downloads: SeeYou Navigator
                {
                    var pathToNewFile = Path.Combine(_downloadPath, downloadedFileName);
                    File.WriteAllText(pathToNewFile, textData);
                }

                if (IsSaveForXcSoar) // XCSoar
                {
                    try
                    {
                        var pathToNewFile = Path.Combine(XCSoarDownloadPath, downloadedFileName);
                        if (fileDescription.IsZip)
                        {
                            // --- ZIP file = write to disk and extract
                            OnLogUpdateRequested?.Invoke(this, $"Writting ZIP: {pathToNewFile}");
                            using var writer = new BinaryWriter(File.OpenWrite(pathToNewFile));
                            writer.Write(data);
                            writer.Flush();
                            writer.Close();
                            writer.Dispose();

                            // Extract to disk
                            // Log
                            OnLogUpdateRequested?.Invoke(this, $"Extracting ZIP: {pathToNewFile} -> {XCSoarDownloadPath}");
                            ZipService.ExtractZipFileToDirectory(pathToNewFile, XCSoarDownloadPath, true);
                        }
                        else 
                        {
                            // Text file: Save to file directly
                            File.WriteAllText(pathToNewFile, textData);
                        }
                    }
                    catch (DirectoryNotFoundException dnfe)
                    {
                        OnFileDownloaded?.Invoke(this, new DownloadEventArgs(fileDescription, false, dnfe.Message));
                    }
                }

                // Notify: 
                FilesDownloadedCount++;
                OnFileDownloaded?.Invoke(this, new DownloadEventArgs(fileDescription, true));
            }
            else
            {
                // Error while downloading the file
                OnFileDownloaded?.Invoke(this, new DownloadEventArgs(fileDescription, false, "No Internet Connection ?"));
                System.Console.WriteLine(e.Error.ToString());
            }
        }

        /// <summary>
        /// Default XCSoar Download path
        /// </summary>
        /// <returns></returns>
        public string GetDfaultXCSoarDownloadPath()
        {
            return Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, "XCSoarData");
        }
    }
}