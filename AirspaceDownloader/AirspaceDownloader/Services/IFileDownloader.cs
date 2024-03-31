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

        string SeeYouDownloadPath { get; set; }
        string SeeYouAirspaceDownloadPath { get; set; }
        string SeeYouBothFilesDownloadPath { get; set; }
        string XCSoarDownloadPath { get; set; }

        void DownloadFiles(List<FileDescription> listUrls);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
        event EventHandler<string> OnLogUpdateRequested;
        string GetDefaultSeeYouDownloadPath();
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