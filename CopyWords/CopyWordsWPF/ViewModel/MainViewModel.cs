using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using CopyWordsWPF.ViewModel.Commands;

namespace CopyWordsWPF.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private WordViewModel _wordViewModel;
        private LookUpWordCommand _lookUpWordCommand;
        private ShowSettingsDialogCommand _showSettingsDialogCommand;        

        private string _lookUp;

        public MainViewModel()
        {
            _wordViewModel = new WordViewModel();
            _lookUpWordCommand = new LookUpWordCommand(this);
            _showSettingsDialogCommand = new ShowSettingsDialogCommand();
        }

        public LookUpWordCommand LookUpWordCommand
        {
            get { return _lookUpWordCommand; }
        }

        public ShowSettingsDialogCommand ShowSettingsDialogCommand
        {
            get { return _showSettingsDialogCommand; }
        }

        /// <summary>
        /// Gets or sets URL to search the word or a word to search.
        /// </summary>
        public string LookUp
        {
            get { return _lookUp; }
            set
            {
                SetProperty<string>(ref _lookUp, value);
            }
        }

        public WordViewModel WordViewModel
        {
            get { return _wordViewModel; }
        }
    }
}
