using System;
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

        private string _downloadedFileName;

        public bool IsSaveForDownloads { get; set; }
        public bool IsSaveForXcSoar { get; set; }
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;


        /// <summary>
        ///     DownloadFile
        /// </summary>
        /// <param name="url"></param>
        public void DownloadFile(string url)
        {
            try
            {
                _downloadedFileName = Path.GetFileName(url);
                // Download
                var webClient = new WebClient();
                webClient.DownloadDataCompleted += DownloadDataCallback;

                webClient.DownloadDataAsync(new Uri(url));
            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false, ex.Message));
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

                // Save to disk
                if (IsSaveForDownloads) // Downloads: SeeYou Navigator
                {
                    var pathToNewFile = Path.Combine(_downloadPath, _downloadedFileName);
                    File.WriteAllText(pathToNewFile, textData);
                }

                if (IsSaveForXcSoar) // XCSoar
                {
                    var pathToNewFile = Path.Combine(_xcsoarPath, _downloadedFileName);
                    File.WriteAllText(pathToNewFile, textData);
                }

                // Notify: 
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