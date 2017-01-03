using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

            this.DataContext = _viewModel;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Validate())
            {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
