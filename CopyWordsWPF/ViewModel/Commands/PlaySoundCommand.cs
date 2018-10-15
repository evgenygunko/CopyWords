using System;
using System.IO;
using System.Net;

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

            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;

            using (WebClient wc = new WebClient())
            {
                using (Stream fileStream = wc.OpenRead(soundURL))
                {
                    player.Load(fileStream);
                    player.Play();
                }
            }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
