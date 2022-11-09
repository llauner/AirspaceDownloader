using Xamarin.Essentials;

namespace AirspaceDownloader.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        /// <summary>
        /// </summary>
        public AboutViewModel()
        {
            Title = "Airspace Downloader";
        }

        public string AppVersion => AppInfo.VersionString;
    }
}