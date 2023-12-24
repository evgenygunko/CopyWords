using System.Windows.Media;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class PlaySoundCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            string soundURL = parameter as string;

            if (!soundURL.EndsWith(".mp3"))
            {
                throw new ArgumentException("Sound file URL doesn't end with '.mp3'");
            }

            Uri soundFileUri;
            if (!Uri.TryCreate(soundURL, UriKind.Absolute, out soundFileUri))
            {
                throw new ArgumentException($"URL for sound file '{soundFileUri}' is invalid.");
            }

            var player = new MediaPlayer();
            player.Open(soundFileUri);
            player.Play();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
