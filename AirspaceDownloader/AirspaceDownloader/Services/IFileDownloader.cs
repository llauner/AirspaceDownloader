using System;

namespace AirspaceDownloader.Services
{
    public interface IFileDownloader
    {
        void DownloadFile(string url, string folder);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }

    public class DownloadEventArgs : EventArgs
    {
        public string ErrorMessage;
        public bool FileSaved;

        public DownloadEventArgs(bool fileSaved, string errorMessage = null)
        {
            FileSaved = fileSaved;
            ErrorMessage = errorMessage;
        }
    }
}