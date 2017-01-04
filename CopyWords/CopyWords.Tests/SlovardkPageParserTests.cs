using System.IO;
using CopyWordsWPF.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyWords.Tests
{
    [TestClass]
    public class SlovardkPageParserTests
    {
        [TestMethod]
        [DeploymentItem(@"TestPages\slovardk\AfgørelsePage.html")]
        public void LoadStream_ReadsInCorrectEncoding()
        {
            string fileContent;

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("AfgørelsePage.html", System.Text.Encoding.GetEncoding(1251)))
            {
                using (var sr = new StreamReader(ms, System.Text.Encoding.GetEncoding(1251)))
                {
                    fileContent = sr.ReadToEnd();
                }
            }

            Assert.IsTrue(fileContent.Contains("решение, улаживание"));
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\slovardk\AfgørelsePage.html")]
        public void ParseWord_ReturnsSingleTranslation_ForAfgørelsePage()
        {
            SlovardkPageParser parser = new SlovardkPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("AfgørelsePage.html", System.Text.Encoding.GetEncoding(1251)))
            {
                parser.LoadStream(ms);
            }

            var translations = parser.ParseWord();
            Assert.AreEqual(1, translations.Count);
            Assert.AreEqual("afgørelse", translations[0].DanishWord);
            Assert.AreEqual("решение, улаживание", translations[0].Translation);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\slovardk\KæmpePage.html")]
        public void ParseWord_ReturnsMultipleTranslations_ForKæmpePage()
        {
            SlovardkPageParser parser = new SlovardkPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("KæmpePage.html", System.Text.Encoding.GetEncoding(1251)))
            {
                parser.LoadStream(ms);
            }

            var translations = parser.ParseWord();

            Assert.AreEqual(3, translations.Count);

            Assert.AreEqual("kæmpe", translations[0].DanishWord);
            Assert.AreEqual(
                "1) богатырь, великан\r\n2) бороться, биться, сражаться",
                translations[0].Translation);

            Assert.AreEqual("kæmpemæssig", translations[1].DanishWord);
            Assert.AreEqual("гигантский", translations[1].Translation);

            Assert.AreEqual("kæmpestor", translations[2].DanishWord);
            Assert.AreEqual("огромный", translations[2].Translation);
        }
    }
}
