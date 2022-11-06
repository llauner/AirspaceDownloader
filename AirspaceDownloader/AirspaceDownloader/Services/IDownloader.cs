using System;
using System.Threading.Tasks;

namespace AirspaceDownloader.Services
{
    public interface IDownloader
    {
        void DownloadFile(string url, string folder);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }

    public class DownloadEventArgs : EventArgs
    {
        public bool FileSaved;
        public string ErrorMessage = null;

        public DownloadEventArgs(bool fileSaved, string errorMessage=null)
        {
            FileSaved = fileSaved;
            ErrorMessage = errorMessage;
        }
    }
}