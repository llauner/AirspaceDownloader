using System;
using System.Collections.Generic;

namespace AirspaceDownloader.Services
{
    public interface IFileDownloader
    {
        bool IsSaveForDownloads { get; set; }
        bool IsSaveForXcSoar { get; set; }
        int NbFilesToDownload { get; }
        int FilesDownloadedCount { get; set; }
        bool IsDownloadBatchFinished { get; }

        void DownloadFiles(List<string> listUrls);
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