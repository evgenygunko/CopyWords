using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyWordsWPF.ViewModel.Commands;

namespace CopyWordsWPF.ViewModel
{
    public class SettingsViewModel : BindableBase, IDataErrorInfo
    {
        private const string AreFieldsNotEmptyProperty = "AreFieldsNotEmpty";
        private const string Mp3gainPathProperty = "Mp3gainPath";
        private const string AnkiSoundsFolderProperty = "AnkiSoundsFolder";

        private string _ankiSoundsFolder;
        private string _mp3gainPath;
        
        private bool _isValidating = false;
        private Dictionary<string, string> _errors = new Dictionary<string, string>();
   
        public SettingsViewModel()
        {
            _ankiSoundsFolder = CopyWordsWPF.Properties.Settings.Default.AnkiSoundsFolder;
            _mp3gainPath = CopyWordsWPF.Properties.Settings.Default.Mp3gainPath;
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

        public bool AreFieldsNotEmpty
        {
            get 
            {
                bool valueIsMissing = string.IsNullOrEmpty(_ankiSoundsFolder) || string.IsNullOrEmpty(_mp3gainPath);
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

            return _errors.Count() == 0;
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
                    if (!(File.Exists(_mp3gainPath) && new FileInfo(_mp3gainPath).Name == "mp3gain.exe"))
                    {
                        result = "'Path to mp3gain' must point to mp3gain.exe.";
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
