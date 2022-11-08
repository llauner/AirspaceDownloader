namespace AirspaceDownloader.Services
{
    internal class DownloaderService : IDownloaderService

    {
        private readonly IFileDownloader _fileDownloader;

        public DownloaderService(IFileDownloader fileDownloader)
        {
            _fileDownloader = fileDownloader;
        }

        public string FileUrl { get; set; } // UrL of the file to be downloaded

        public void DownloadFile(string url, string folder)
        {
            _fileDownloader.DownloadFile(url, folder);
        }
    }
}