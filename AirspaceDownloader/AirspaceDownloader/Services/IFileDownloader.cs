using System;

namespace AirspaceDownloader.Services
{
    public interface IFileDownloader
    {
        bool IsSaveForDownloads { get; set; }
        bool IsSaveForXcSoar { get; set; }

        void DownloadFile(string url);
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