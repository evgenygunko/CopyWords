using System.Windows;
using CopyWordsWPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace CopyWordsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            DataContext = App.Current.Services.GetService<MainViewModel>();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CopyWordsWPF.Properties.Settings.Default.Save();
        }
    }
}
