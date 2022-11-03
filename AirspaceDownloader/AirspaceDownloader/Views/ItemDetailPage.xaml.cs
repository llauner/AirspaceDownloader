using AirspaceDownloader.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AirspaceDownloader.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}