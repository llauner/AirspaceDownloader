using Xamarin.Essentials;

namespace AirspaceDownloader.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        /// <summary>
        /// </summary>
        public AboutViewModel()
        {
            Title = "github.com/planeur-net Downloader";
        }

        public string AppVersion => AppInfo.VersionString;
    }
}