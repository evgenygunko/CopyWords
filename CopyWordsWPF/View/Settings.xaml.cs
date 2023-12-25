using System.Windows;
using CopyWordsWPF.ViewModel;

namespace CopyWordsWPF.View
{
    /// <summary>
    /// Interaction logic for Settings.
    /// </summary>
    public partial class Settings : Window
    {
        public Settings(SettingsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.OnRequestClose += (s, e) => Close();
        }
    }
}
