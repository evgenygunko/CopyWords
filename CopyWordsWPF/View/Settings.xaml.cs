using System.Windows;
using CopyWordsWPF.ViewModel;

namespace CopyWordsWPF.View
{
    /// <summary>
    /// Interaction logic for Settings.
    /// </summary>
    public partial class Settings : Window
    {
        private SettingsViewModel _viewModel = new SettingsViewModel();

        public Settings()
        {
            InitializeComponent();

            DataContext = _viewModel;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Validate())
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
