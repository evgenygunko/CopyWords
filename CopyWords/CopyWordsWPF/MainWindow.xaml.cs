using System.Windows;
using CopyWordsWPF.ViewModel;

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

            DataContext = new MainViewModel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CopyWordsWPF.Properties.Settings.Default.Save();
        }
    }
}
