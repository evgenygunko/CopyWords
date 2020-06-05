using System;
using System.Threading.Tasks;
using System.Windows;
using CopyWords.Parsers;
using CopyWords.Parsers.Models;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class LookUpWordCommand : CommandBase
    {
        private MainViewModel _mainViewModel;

        public LookUpWordCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public async override void Execute(object parameter)
        {
            string lookUp = _mainViewModel.LookUp;
            if (string.IsNullOrEmpty(lookUp))
            {
                MessageBox.Show("LookUp text can't be null or empty.", "Incorrect input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            WordModel wordModel = await LookUpWordAsync(lookUp);

            if (wordModel != null)
            {
                try
                {
                    WordViewModel wordViewModel = _mainViewModel.WordViewModel;
                    wordViewModel.Word = wordModel.Word;
                    wordViewModel.Endings = wordModel.Endings;
                    wordViewModel.Pronunciation = wordModel.Pronunciation;
                    wordViewModel.Sound = wordModel.Sound;
                    wordViewModel.Definitions = wordModel.Definitions;
                    wordViewModel.Examples = wordModel.Examples;
                    wordViewModel.RussianTranslations = wordModel.Translations;
                }
                catch (Exception ex)
                {
                    var logger = NLog.LogManager.GetCurrentClassLogger();
                    logger.Error(ex, "Could not parse the result");

                    MessageBox.Show("Could not parse the result. See the log file for details. Error: " + ex.Message, "Error while parsing result", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private static async Task<WordModel> LookUpWordAsync(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return null;
            }

            LookUpWord command = new LookUpWord();

            (bool isValid, string errorMessage) = command.CheckThatWordIsValid(word);
            if (!isValid)
            {
                MessageBox.Show(errorMessage, "Invalid search term", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            WordModel wordModel = null;
            try
            {
                wordModel = await command.LookUpWordAsync(word);

                if (wordModel == null)
                {
                    MessageBox.Show($"Den Danske Ordbog doesn't have a page for '{word}'", "Cannot find word", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occurred while searching word", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            return wordModel;
        }
    }
}
