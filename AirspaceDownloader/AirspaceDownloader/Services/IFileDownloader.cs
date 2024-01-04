using AirspaceDownloader.Models;
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
        string XCSoarDownloadPath { get; set; }

        void DownloadFiles(List<FileDescription> listUrls);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
        string GetDfaultXCSoarDownloadPath();
    }

    public class DownloadEventArgs : EventArgs
    {
        public string ErrorMessage;
        public bool FileSaved;
        public FileDescription FileDescription;

        public DownloadEventArgs(FileDescription fileDescription, bool fileSaved, string errorMessage = null)
        {
            FileDescription = fileDescription;
            FileSaved = fileSaved;
            ErrorMessage = errorMessage;
        }
    }
}