using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CopyWordsWPF.Parsers
{
    public abstract class PageParserBase
    {
        private HtmlDocument _htmlDocument;

        protected HtmlDocument PageHtmlDocument
        {
            get { return _htmlDocument; }
        }

        public static string DecodeText(string innerText)
        {
            return WebUtility.HtmlDecode(innerText).Trim();
        }

        public void LoadStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException();
            }

            _htmlDocument = new HtmlDocument();
            _htmlDocument.Load(stream);

            if (_htmlDocument.DocumentNode == null)
            {
                throw new PageParserException("DocumentNode is null for the loaded stream, please check that it has a valid html content.");
            }
        }

        public HtmlNode FindElementByClassName(string elementName, string className)
        {
            var elements = FindElementsByClassName(elementName, className);
            return elements.First();
        }

        public HtmlNodeCollection FindElementsByClassName(string elementName, string className)
        {
            var elements = _htmlDocument.DocumentNode.SelectNodes(
                string.Format("//{0}[contains(@class, '{1}')]", elementName, className));

            if (elements == null)
            {
                throw new PageParserException(string.Format("Cannot find any element '{0}' with CSS class '{1}'", elementName, className));
            }

            return elements;
        }

        public HtmlNode FindElementById(string id)
        {
            var element = _htmlDocument.DocumentNode.SelectSingleNode(string.Format("//*[@id='{0}']", id));
            return element;
        }       
    }
}
