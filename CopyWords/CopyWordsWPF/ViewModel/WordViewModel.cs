using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyWordsWPF.Parsers;
using CopyWordsWPF.ViewModel.Commands;

namespace CopyWordsWPF.ViewModel
{
    public class WordViewModel : BindableBase
    {
        private string _word = "<>";
        private string _endings = "<>";
        private string _pronunciation = "<>";
        private string _sound = "<>";
        private string _definitions = "<>";
        private List<string> _examples = new List<string>();
        private List<RussianTranslation> _russianTranslations = new List<RussianTranslation>();
        
        private CopyTextCommand _copyTextCommand;
        private PlaySoundCommand _playSoundCommand;
        private CopySoundFileCommand _copySoundFileCommand;
        private LookupInDRDictCommand _lookupInDRDictCommand;

        public event EventHandler FileCopied;

        public WordViewModel()
        {
            _copyTextCommand = new CopyTextCommand();
            _playSoundCommand = new PlaySoundCommand();
            _copySoundFileCommand = new CopySoundFileCommand(this);
            _lookupInDRDictCommand = new LookupInDRDictCommand();
        }

        #region Properties

        #region Commands

        public CopyTextCommand CopyTextCommand
        {
            get { return _copyTextCommand; }
        }

        public PlaySoundCommand PlaySoundCommand
        {
            get { return _playSoundCommand; }
        }

        public CopySoundFileCommand CopySoundFileCommand
        {
            get { return _copySoundFileCommand; }
        }

        public LookupInDRDictCommand LookupInDRDictCommand
        {
            get { return _lookupInDRDictCommand; }
        }

        #endregion

        #region Parsed values

        public string Word
        {
            get { return _word; }
            set
            {
                SetProperty<string>(ref _word, value);
            }
        }

        public string Endings
        {
            get { return _endings; }
            set
            {
                SetProperty<string>(ref _endings, value);
            }
        }

        public string Pronunciation
        {
            get { return _pronunciation; }
            set
            {
                SetProperty<string>(ref _pronunciation, value);
            }
        }

        public string Sound
        {
            get { return _sound; }
            set
            {
                SetProperty<string>(ref _sound, value);
            }
        }

        public string Definitions
        {
            get { return _definitions; }
            set
            {
                SetProperty<string>(ref _definitions, value);
            }
        }

        public string ExamplesString
        {
            get
            {
                if (_examples.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    string delimeter = Environment.NewLine;
                    string examples = _examples.Aggregate((i, j) => i + delimeter + j);

                    return examples; 
                }
            }
        }

        public List<string> Examples
        {
            get { return _examples; }
            set
            {
                if (SetProperty<List<string>>(ref _examples, value))
                {
                    OnPropertyChanged("ExamplesString");
                }
            }
        }

        public List<RussianTranslation> RussianTranslations
        {
            get { return _russianTranslations; }
            set
            {
                SetProperty<List<RussianTranslation>>(ref _russianTranslations, value);
            }
        }

        public void OnFileCopied()
        {
            var eventHandler = FileCopied;
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }

        #endregion 

        #endregion
    }
}
