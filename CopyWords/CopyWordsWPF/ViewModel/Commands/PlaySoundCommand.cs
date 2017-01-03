using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WMPLib;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class PlaySoundCommand : CommandBase
    {
        private WindowsMediaPlayer _player;
        private string _soundURL;       

        public override void Execute(object parameter)
        {
            _soundURL = parameter as string;

            if (!_soundURL.EndsWith(".mp3"))
            {
                throw new ArgumentException("Sound file URL doesn't end with '.mp3'");
            }

            _player = new WindowsMediaPlayer();
            _player.MediaError += player_MediaError;
            _player.PlayStateChange += player_PlayStateChange;

            _player.URL = _soundURL;
            _player.controls.play();
        }

        public override bool CanExecute(object parameter)
        {
            return _player == null;
        }

        private void player_PlayStateChange(int newState)
        {
            if ((WMPLib.WMPPlayState)newState == WMPLib.WMPPlayState.wmppsStopped)
            {
                _player.MediaError -= player_MediaError;
                _player.PlayStateChange -= player_PlayStateChange;
                _player.close();

                _player = null;
            }
        }

        private void player_MediaError(object mediaObject)
        {
            throw new Exception(string.Format("Cannot play media file '{0}'.", _soundURL));
        }
    }
}
