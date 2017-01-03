using System;
using System.Windows;
using System.Windows.Input;
using CopyWordsWPF.View;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class ShowSettingsDialogCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            Settings settingsDialog = new Settings();
            if (settingsDialog.ShowDialog() == true)
            {
                // save settings
                SettingsViewModel vm = settingsDialog.DataContext as SettingsViewModel;

                CopyWordsWPF.Properties.Settings.Default.AnkiSoundsFolder = vm.AnkiSoundsFolder;
                CopyWordsWPF.Properties.Settings.Default.Mp3gainPath = vm.Mp3gainPath;            
            }
        }
    }
}