using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using CopyWordsWPF.Services;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class CopySoundFileCommand : CommandBase
    {
        private readonly HttpClient _httpClient;
        private readonly ISettingsService _settingsService;
        private readonly WordViewModel _wordViewModel;

        private readonly PlaySoundCommand _playSound;

        public CopySoundFileCommand(
            HttpClient httpClient,
            ISettingsService settingsService,
            WordViewModel wordViewModel)
        {
            _httpClient = httpClient;
            _settingsService = settingsService;
            _wordViewModel = wordViewModel;

            _playSound = new PlaySoundCommand();
        }

        public override void Execute(object parameter)
        {
            if (!_wordViewModel.Sound.EndsWith(".mp3"))
            {
                throw new ArgumentException(
                    string.Format("Sound file URL doesn't end with '.mp3'. _soundURL = '{0}'", _wordViewModel.Sound));
            }

            Uri soundFileUri;
            if (!Uri.TryCreate(_wordViewModel.Sound, UriKind.Absolute, out soundFileUri))
            {
                throw new ArgumentException(string.Format("URL for sound file '{0}' is invalid.", _wordViewModel.Sound));
            }

            if (string.IsNullOrEmpty(_settingsService.GetAnkiSoundsFolder()) || !Directory.Exists(_settingsService.GetAnkiSoundsFolder()))
            {
                MessageBox.Show(
                    string.Format("'AnkiSoundsFolder' parameter in Settings must contain path to an existing folder. Please select a valid path in Settings."),
                    "Cannot copy mp3 file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            string fromFile = null;
            Task.Run(async () =>
            {
                fromFile = await DownloadFileAsync(soundFileUri, _wordViewModel.Word);
            }).Wait();

            // normalize mp3 file
            if (_settingsService.UseMp3gain)
            {
                if (!CallMp3gain(fromFile))
                {
                    return;
                }
            }

            string destinationFile = Path.Combine(_settingsService.GetAnkiSoundsFolder(), _wordViewModel.Word + ".mp3");
            Debug.Assert(destinationFile.Length > 0, "Path to destination file can't be empty.");

            // save text for Anki into clipboard
            Clipboard.SetText(string.Format("[sound:{0}.mp3]", _wordViewModel.Word));

            // copy file into Anki's sounds folder
            if (CopyWord(fromFile, destinationFile))
            {
                if (_playSound.CanExecute(parameter))
                {
                    // play normalized file
                    _playSound.Execute(fromFile);
                }

                _wordViewModel.OnFileCopied();
            }
        }

        private static bool CopyWord(string source, string destination)
        {
            if (File.Exists(destination))
            {
                if (MessageBox.Show(
                    string.Format("File '{0}' already exists. Overwrite?", destination),
                    "File already exists",
                    MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return false;
                }
            }

            File.Copy(source, destination, true);
            return true;
        }

        private bool CallMp3gain(string fromFile)
        {
            if (!File.Exists(_settingsService.GetMp3gainPath()))
            {
                MessageBox.Show(
                    string.Format("Cannot find mp3gain.exe by path '{0}'. Please select a valid path in Settings.", _settingsService.GetMp3gainPath()),
                    "Cannot normalize mp3 file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            // m3gain doesn't support unicode file names - we will use a temp name and then rename it back to desired file name
            string tempAsciiFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mp3");
            File.Copy(fromFile, tempAsciiFile);

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = _settingsService.GetMp3gainPath();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.Arguments = "-r " + tempAsciiFile;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "mp3gain threw exception: " + Environment.NewLine + ex.ToString(),
                    "Cannot normalize mp3 file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }

            File.Copy(tempAsciiFile, fromFile, true);
            File.Delete(tempAsciiFile);

            return true;
        }

        private async Task<string> DownloadFileAsync(Uri soundFileUri, string destFileName)
        {
            string destFileFullPath = Path.Combine(Path.GetTempPath(), destFileName + ".mp3");

            // do not download file again if it is already downloaded
            if (!File.Exists(destFileFullPath))
            {
                using (var result = await _httpClient.GetAsync(soundFileUri))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        byte[] fileBytes = await result.Content.ReadAsByteArrayAsync();
                        await File.WriteAllBytesAsync(destFileFullPath, fileBytes);
                    }
                }
            }

            if (!File.Exists(destFileFullPath))
            {
                throw new FileDownloadException($"Cannot find sound file in a temp folder '{destFileFullPath}'. It probably hasn't been downloaded.");
            }

            return destFileFullPath;
        }
    }
}
