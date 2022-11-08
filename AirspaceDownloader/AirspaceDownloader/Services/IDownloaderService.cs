namespace AirspaceDownloader.Services
{
    public interface IDownloaderService
    {
        string FileUrl { get; set; } // UrL of the file to be downloaded

        void DownloadFile(string url, string folder);
    }
}