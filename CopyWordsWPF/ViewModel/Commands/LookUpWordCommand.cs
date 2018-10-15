using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using CopyWords.Parsers;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class LookUpWordCommand : CommandBase
    {
        private readonly HttpClient _httpClient = new HttpClient();

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
                throw new ArgumentException("LookUp text can't be null or empty.");
            }

            string ddoUrl;
            string slovardkUrl;
            string wordToLookUp = lookUp;

            if (Uri.IsWellFormedUriString(lookUp, UriKind.Absolute))
            {
                if (!lookUp.StartsWith("http://ordnet.dk/ddo/ordbog"))
                {
                    MessageBox.Show("Incorrect url to lookup a word.", "Incorrect url", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ddoUrl = lookUp;
                wordToLookUp = GetWordToLookupFromUrl(lookUp);
            }
            else
            {
                if (!CheckThatWordIsValid(lookUp))
                {
                    MessageBox.Show("Search can only contain alphanumeric characters and spaces.", "Invalid search term", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ddoUrl = string.Format("http://ordnet.dk/ddo/ordbog?query={0}&search=S%C3%B8g", lookUp);
            }

            slovardkUrl = GetSlovardkUri(wordToLookUp);

            // Download and parse a page from DDO
            string ddoPageHtml = await DownloadPageAsync(ddoUrl, Encoding.UTF8);
            if (string.IsNullOrEmpty(ddoPageHtml))
            {
                return;
            }

            DDOPageParser ddoPageParser = new DDOPageParser();
            ddoPageParser.LoadHtml(ddoPageHtml);

            WordViewModel wordViewModel = _mainViewModel.WordViewModel;
            wordViewModel.Word = ddoPageParser.ParseWord();
            wordViewModel.Endings = ddoPageParser.ParseEndings();
            wordViewModel.Pronunciation = ddoPageParser.ParsePronunciation();
            wordViewModel.Sound = ddoPageParser.ParseSound();
            wordViewModel.Definitions = ddoPageParser.ParseDefinitions();
            wordViewModel.Examples = ddoPageParser.ParseExamples();

            // Download and parse a page from Slovar.dk
            string slovardkPageHtml = await DownloadPageAsync(slovardkUrl, Encoding.GetEncoding(1251));

            SlovardkPageParser slovardkPageParser = new SlovardkPageParser();
            slovardkPageParser.LoadHtml(slovardkPageHtml);

            var translations = slovardkPageParser.ParseWord();
            wordViewModel.RussianTranslations = translations;
        }

        internal static bool CheckThatWordIsValid(string lookUp)
        {
            Regex regex = new Regex(@"^[\w ]+$");
            return regex.IsMatch(lookUp);
        }

        internal static string GetSlovardkUri(string wordToLookUp)
        {
            wordToLookUp = wordToLookUp.ToLower()
                .Replace("å", "'aa")
                .Replace("æ", "'ae")
                .Replace("ø", "'oe")
                .Replace(" ", "-");

            string uri = string.Format("http://www.slovar.dk/tdansk/{0}/?", wordToLookUp);
            return uri;
        }

        internal static string GetWordToLookupFromUrl(string lookUpUri)
        {
            Uri myUri = new Uri(lookUpUri);
            string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("query");

            if (!string.IsNullOrEmpty(param1))
            {
                return param1.ToLower();
            }
            else
            {
                return string.Empty;
            }
        }

        private async Task<string> DownloadPageAsync(string url, Encoding encoding)
        {
            string content = null;

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                content = encoding.GetString(bytes, 0, bytes.Length - 1);
            }
            else
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    throw new ServerErrorException("Server returned " + response.StatusCode);
                }
            }

            return content;
        }
    }
}
