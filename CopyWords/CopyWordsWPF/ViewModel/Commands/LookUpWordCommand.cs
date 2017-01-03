using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using CopyWordsWPF.Parsers;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class LookUpWordCommand : CommandBase
    {
        private MainViewModel _mainViewModel;

        public LookUpWordCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object parameter)
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
            Stream ddoStream = DownloadPage(ddoUrl); 
            if (ddoStream == null)
            {
                return;
            }

            DDOPageParser ddoPageParser = new DDOPageParser();
            ddoPageParser.LoadStream(ddoStream);

            WordViewModel wordViewModel = _mainViewModel.WordViewModel;
            wordViewModel.Word = ddoPageParser.ParseWord();
            wordViewModel.Endings = ddoPageParser.ParseEndings();
            wordViewModel.Pronunciation = ddoPageParser.ParsePronunciation();
            wordViewModel.Sound = ddoPageParser.ParseSound();
            wordViewModel.Definitions = ddoPageParser.ParseDefinitions();
            wordViewModel.Examples = ddoPageParser.ParseExamples();

            // Download and parse a page from Slovar.dk
            Stream slovardkStream = DownloadPage(slovardkUrl);
            SlovardkPageParser slovardkPageParser = new SlovardkPageParser();
            slovardkPageParser.LoadStream(slovardkStream);

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

        private static Stream DownloadPage(string url)
        {
            //Create a WebRequest to get the file
            HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

            HttpWebResponse fileResp = null;

            try
            {
                //Create a response for this request
                fileResp = (HttpWebResponse)fileReq.GetResponse();
            }
            catch (WebException ex)
            {
                HttpWebResponse errorResponse = ex.Response as HttpWebResponse;
                if (errorResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Den Danske Ordbog server returned NotFound exception.", "Cannot find word", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }
            }

            if (fileReq.ContentLength > 0)
            {
                fileResp.ContentLength = fileReq.ContentLength;
            }

            //Get the Stream returned from the response
            Stream stream = fileResp.GetResponseStream();
            return stream;
        }        
    }
}
