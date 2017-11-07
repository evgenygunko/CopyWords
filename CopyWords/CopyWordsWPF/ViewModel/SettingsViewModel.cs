using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace CopyWordsWPF.ViewModel
{
    public class SettingsViewModel : BindableBase, IDataErrorInfo
    {
        private const string AreFieldsNotEmptyProperty = "AreFieldsNotEmpty";
        private const string Mp3gainPathProperty = "Mp3gainPath";
        private const string AnkiSoundsFolderProperty = "AnkiSoundsFolder";
        private const string UseMp3gainProperty = "UseMp3gain";

        private string _ankiSoundsFolder;
        private string _mp3gainPath;

        private bool _isValidating = false;
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
        private bool _useMp3gain = true;

        public SettingsViewModel()
        {
            _ankiSoundsFolder = CopyWordsWPF.Properties.Settings.Default.AnkiSoundsFolder;
            _mp3gainPath = CopyWordsWPF.Properties.Settings.Default.Mp3gainPath;
            _useMp3gain = CopyWordsWPF.Properties.Settings.Default.UseMp3gain;
        }

        /// <summary>
        /// Gets or sets path to collection.media in Anki.
        /// </summary>
        public string AnkiSoundsFolder
        {
            get { return _ankiSoundsFolder; }
            set
            {
                if (SetProperty<string>(ref _ankiSoundsFolder, value))
                {
                    OnPropertyChanged(AreFieldsNotEmptyProperty);
                }
            }
        }

        /// <summary>
        /// Gets or sets path to mp3gain.exe.
        /// </summary>
        public string Mp3gainPath
        {
            get { return _mp3gainPath; }
            set
            {
                if (SetProperty<string>(ref _mp3gainPath, value))
                {
                    OnPropertyChanged(AreFieldsNotEmptyProperty);
                }
            }
        }

        public bool UseMp3gain
        {
            get { return _useMp3gain; }
            set
            {
                if (SetProperty<bool>(ref _useMp3gain, value))
                {
                    OnPropertyChanged(UseMp3gainProperty);
                    OnPropertyChanged(AreFieldsNotEmptyProperty);
                }
            }
        }

        public bool AreFieldsNotEmpty
        {
            get
            {
                bool valueIsMissing = string.IsNullOrEmpty(_ankiSoundsFolder) ||
                    (_useMp3gain && string.IsNullOrEmpty(_mp3gainPath));
                return !valueIsMissing;
            }
        }

        public bool Validate()
        {
            _isValidating = true;
            try
            {
                OnPropertyChanged(AnkiSoundsFolderProperty);
                OnPropertyChanged(Mp3gainPathProperty);
            }
            finally
            {
                _isValidating = false;
            }

            return _errors.Count == 0;
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;

                if (!_isValidating)
                {
                    return result;
                }

                _errors.Remove(columnName);

                if (columnName == AnkiSoundsFolderProperty)
                {
                    if (!Directory.Exists(_ankiSoundsFolder))
                    {
                        result = "'Path to Anki sounds folder' must point to existing directory.";
                    }
                }

                if (columnName == Mp3gainPathProperty)
                {
                    if (_useMp3gain)
                    {
                        if (!(File.Exists(_mp3gainPath) && new FileInfo(_mp3gainPath).Name == "mp3gain.exe"))
                        {
                            result = "'Path to mp3gain' must point to mp3gain.exe.";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(result))
                {
                    _errors.Add(columnName, result);
                }

                return result;
            }
        }

        #endregion
    }
}
