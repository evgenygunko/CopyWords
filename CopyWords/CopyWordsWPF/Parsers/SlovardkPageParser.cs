using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace CopyWordsWPF.Parsers
{
    public class SlovardkPageParser : PageParserBase
    {
        /// <summary>
        /// Gets a string which contains found Danish word.
        /// </summary>
        public List<RussianTranslation> ParseWord()
        {
            List<RussianTranslation> russianTranslations = new List<RussianTranslation>();

            var tds = PageHtmlDocument.DocumentNode.SelectNodes("//td[@valign='middle']");

            if (tds != null)
            {
                foreach (var td in tds)
                {
                    if (td.ChildNodes != null && td.ChildNodes[0].NodeType == HtmlNodeType.Text)
                    {
                        string translations = td.InnerHtml.Trim();
                        if (translations.Length > 0 && !translations.Contains("You need to upgrade your Flash Player"))
                        {
                            string translation = string.Empty;
                            foreach (var s in translations.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                translation += s.Trim() + Environment.NewLine;
                            }

                            translation = translation.Trim();

                            var danishWordElement = td.ParentNode.FirstChild.SelectSingleNode(".//b");
                            string danishWord = string.Empty;

                            if (danishWordElement != null)
                            {
                                danishWord = DecodeText(danishWordElement.InnerText);
                            }

                            russianTranslations.Add(new RussianTranslation(danishWord, translation));
                        }
                    }
                }
            }

            return russianTranslations;
        }
    }
}
