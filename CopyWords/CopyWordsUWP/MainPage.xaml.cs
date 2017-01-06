using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CopyWordsUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            object lookupText;
            if (SuspensionManager.SessionState.TryGetValue("txtLookUp", out lookupText))
            {
                txtLookUp.Text = lookupText?.ToString();
            }
        }

        private void txtLookUp_TextChanged(object sender, TextChangedEventArgs e)
        {
            SuspensionManager.SessionState["txtLookUp"] = ((TextBox)sender).Text;
        }
    }
}
