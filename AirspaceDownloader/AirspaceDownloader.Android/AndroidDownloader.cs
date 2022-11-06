using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AirspaceDownloader.Droid;
using AirspaceDownloader.Services;
using Xamarin.Forms;
using Environment = Android.OS.Environment;

[assembly: Dependency(typeof(AndroidDownloader))]

namespace AirspaceDownloader.Droid
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public void DownloadFile(string url, string folder)
        {
            var pathToNewFolder = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDocuments).AbsolutePath;

            try
            {
                var webClient = new WebClient();
                webClient.DownloadFileCompleted += Completed;
                var pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false, ex.Message));
            }
        }

        /// <summary>
        /// Completed
        /// Download completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false, e.Error.Message));
            }
            else
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
            }
        }
    }
}