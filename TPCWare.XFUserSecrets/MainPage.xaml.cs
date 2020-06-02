using System.ComponentModel;
using TPCWare.XFUserSecrets.Utils;
using Xamarin.Forms;

namespace TPCWare.XFUserSecrets
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var mySecret = UserSecretsManager.Settings["MySecret"];

            // The Answer to the Ultimate Question of Life, the Universe, and Everything
            UserSecretsLabel.Text = $"My secret is {mySecret}";
        }
    }
}






















