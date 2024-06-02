using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CopyWordsWPF.Services;
using Microsoft.Win32;

namespace CopyWordsWPF.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        public event EventHandler OnRequestClose;

        private readonly ISettingsService _settingsService;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            AnkiSoundsFolder = _settingsService.GetAnkiSoundsFolder();
            UseMp3gain = _settingsService.UseMp3gain;
            Mp3gainPath = _settingsService.GetMp3gainPath();
            DanRusDictionaryFolder = _settingsService.GetDanRusDictionaryFolder();
            UseSlovardk = _settingsService.UseSlovardk;
        }

        #region Properties

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveSettingsCommand))]
        private string ankiSoundsFolder;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveSettingsCommand))]
        private bool useMp3gain;

        public bool CanUseMp3gain => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveSettingsCommand))]
        private string mp3gainPath;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveSettingsCommand))]
        private string danRusDictionaryFolder;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveSettingsCommand))]
        private bool useSlovardk;

        public string About => $".net version: {RuntimeInformation.FrameworkDescription}";

        public bool CanSaveSettings
        {
            get
            {
                bool result = Directory.Exists(AnkiSoundsFolder);
                if (UseMp3gain)
                {
                    result &= File.Exists(Mp3gainPath);
                }

                if (!string.IsNullOrEmpty(DanRusDictionaryFolder))
                {
                    result &= Directory.Exists(DanRusDictionaryFolder);
                }

                return result;
            }
        }

        #endregion

        #region Commands

        [RelayCommand]
        public void PickAnkiSoundsFolder()
        {
            var dlg = new OpenFolderDialog
            {
                Title = "Select Folder"
            };

            if (dlg.ShowDialog() == true)
            {
                AnkiSoundsFolder = dlg.FolderName;
            }
        }

        [RelayCommand]
        public void PickMp3gainPath()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Select path to mp3gain.exe",
                Filter = "exe files (*.exe)|*.exe"
            };

            if (dlg.ShowDialog() == true)
            {
                Mp3gainPath = dlg.FileName;
            }
        }

        [RelayCommand]
        public void PickDanRusDictionaryFolder()
        {
            var dlg = new OpenFolderDialog
            {
                Title = "Select Folder"
            };

            if (dlg.ShowDialog() == true)
            {
                DanRusDictionaryFolder = dlg.FolderName;
            }
        }

        [RelayCommand(CanExecute = nameof(CanSaveSettings))]
        public void SaveSettings()
        {
            _settingsService.SetAnkiSoundsFolder(AnkiSoundsFolder);
            _settingsService.UseMp3gain = UseMp3gain;
            _settingsService.SetMp3gainPath(Mp3gainPath);
            _settingsService.SetDanRusDictionaryFolder(DanRusDictionaryFolder);
            _settingsService.UseSlovardk = UseSlovardk;

            _settingsService.Save();

            MessageBox.Show("Settings have been updated", "Update Settings", MessageBoxButton.OK, MessageBoxImage.Information);

            OnRequestClose(this, new EventArgs());
        }

        #endregion
    }
}
