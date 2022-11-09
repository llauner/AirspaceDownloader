using Xamarin.Essentials;
using Xamarin.Forms;

namespace AirspaceDownloader.UserInterface
{
    public class HyperlinkLabel : Label
    {
        public static readonly BindableProperty UrlProperty =
            BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkLabel));

        public HyperlinkLabel()
        {
            TextDecorations = TextDecorations.Underline;
            TextColor = Color.Blue;
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => await Launcher.OpenAsync(Url))
            });
        }

        public string Url
        {
            get => (string) GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }
    }
}