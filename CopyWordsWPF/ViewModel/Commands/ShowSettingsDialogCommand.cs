using CopyWordsWPF.View;
using Microsoft.Extensions.DependencyInjection;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class ShowSettingsDialogCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            Settings settingsDialog = new Settings(App.Current.Services.GetService<SettingsViewModel>());
            settingsDialog.ShowDialog();
        }
    }
}