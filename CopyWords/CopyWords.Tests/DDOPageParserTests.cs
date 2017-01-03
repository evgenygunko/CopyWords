using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CopyWordsWPF;
using CopyWordsWPF.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyWords.Tests
{
    [TestClass]
    public class DDOPageParserTests
    {
        #region LoadStream tests

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\SimplePage.html")]
        public void LoadStream_DoesntThowExpception_ForValidStream()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("SimplePage.html"))
            {
                parser.LoadStream(ms);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadStream_ThowsExpception_ForNullStream()
        {
            DDOPageParser parser = new DDOPageParser();
            parser.LoadStream(null);
        } 

        #endregion

        #region ParseWord tests

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\UnderholdningPage.html")]
        public void ParseWord_ReturnsUnderholdning_ForUnderholdningPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("UnderholdningPage.html"))
            {
                parser.LoadStream(ms);
            }

            string word = parser.ParseWord();
            Assert.AreEqual("underholdning", word);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\GrillspydPage.html")]
        public void ParseWord_ReturnsGrillspydPage_ForUGrillspydPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("GrillspydPage.html"))
            {
                parser.LoadStream(ms);
            }

            string word = parser.ParseWord();
            Assert.AreEqual("grillspyd", word);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\StødtandPage.html")]
        public void ParseWord_ReturnsStødtand_ForUnderholdningPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("StødtandPage.html"))
            {
                parser.LoadStream(ms);
            }

            string word = parser.ParseWord();
            Assert.AreEqual("stødtand", word);
        }

        #endregion

        #region ParseEndings tests

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\UnderholdningPage.html")]
        public void ParseEndings_ReturnsEn_ForUnderholdningPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("UnderholdningPage.html"))
            {
                parser.LoadStream(ms);
            }

            string endings = parser.ParseEndings();
            Assert.AreEqual("-en", endings);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\StødtandPage.html")]
        public void ParseEndings_ReturnsEndings_ForStødtandPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("StødtandPage.html"))
            {
                parser.LoadStream(ms);
            }

            string endings = parser.ParseEndings();
            Assert.AreEqual("-en, ..tænder, ..tænderne", endings);
        }

        #endregion

        #region ParsePronunciation tests

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\UnderholdningPage.html")]
        public void ParsePronunciation_ReturnsUnderholdning_ForUnderholdningPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("UnderholdningPage.html"))
            {
                parser.LoadStream(ms);
            }

            string pronunciation = parser.ParsePronunciation();
            Assert.AreEqual("[ˈɔnʌˌhʌlˀneŋ]", pronunciation);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\KiggePage.html")]
        public void ParsePronunciation_ReturnsKigge_ForKiggePage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("KiggePage.html"))
            {
                parser.LoadStream(ms);
            }

            string pronunciation = parser.ParsePronunciation();
            Assert.AreEqual("[ˈkigə]", pronunciation);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\GrillspydPage.html")]
        public void ParsePronunciation_ReturnsEmptyString_ForGrillspydPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("GrillspydPage.html"))
            {
                parser.LoadStream(ms);
            }

            string pronunciation = parser.ParsePronunciation();
            Assert.AreEqual(string.Empty, pronunciation);
        }

        #endregion

        #region ParseSound tests

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\UnderholdningPage.html")]
        public void ParseSound_ReturnsSoundFile_ForUnderholdningPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("UnderholdningPage.html"))
            {
                parser.LoadStream(ms);
            }

            string pronunciation = parser.ParseSound();
            Assert.AreEqual("http://static.ordnet.dk/mp3/12004/12004770_1.mp3", pronunciation);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\KiggePage.html")]
        public void ParseSound_ReturnsEmptyString_ForKiggePage()
        {
            // Kigge page doesn't have a sound file
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("KiggePage.html"))
            {
                parser.LoadStream(ms);
            }

            string sound = parser.ParseSound();
            Assert.AreEqual(string.Empty, sound);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\DannebrogPage.html")]
        public void ParseSound_ReturnsSoundFile_ForDannebrogPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("DannebrogPage.html"))
            {
                parser.LoadStream(ms);
            }

            string sound = parser.ParseSound();
            Assert.AreEqual("http://static.ordnet.dk/mp3/11008/11008357_1.mp3", sound);
        }

        #endregion

        #region ParseDefinitions tests

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\UnderholdningPage.html")]
        public void ParseDefinitions_ReturnsDefinition_ForUnderholdningPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("UnderholdningPage.html"))
            {
                parser.LoadStream(ms);
            }

            string definitions = parser.ParseDefinitions();

            const string Expected = "noget der morer, glæder eller adspreder nogen, fx optræden, et lettere og ikke særlig krævende åndsprodukt eller en fornøjelig beskæftigelse";
            Assert.AreEqual(Expected, definitions);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\KiggePage.html")]
        public void ParseDefinitions_ReturnsConcatenatedDefinitions_ForKiggePage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("KiggePage.html"))
            {
                parser.LoadStream(ms);
            }

            string definitions = parser.ParseDefinitions();

            string expected = "1. rette blikket i en bestemt retning" + Environment.NewLine +
                "2. undersøge nærmere; sætte sig ind i" + Environment.NewLine +
                "3. prøve at finde" + Environment.NewLine +
                "4. skrive af efter nogen; kopiere noget" + Environment.NewLine +
                "5. se på; betragte";
            Assert.AreEqual(expected, definitions);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\GrillspydPage.html")]
        public void ParseDefinitions_ReturnsEmptyString_ForGrillspydPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("GrillspydPage.html"))
            {
                parser.LoadStream(ms);
            }

            string definitions = parser.ParseDefinitions();

            Assert.AreEqual(string.Empty, definitions);
        }

        #endregion

        #region ParseExamples tests

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\DannebrogPage.html")]
        public void ParseExamples_ReturnsExample_ForDannebrogPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("DannebrogPage.html"))
            {
                parser.LoadStream(ms);
            }

            string expected = "1. [slidt] er det lille dannebrog, der vajende fra sin hvide flagstang pryder pudens forside.";

            List<string> examples = parser.ParseExamples();

            Assert.AreEqual(1, examples.Count);
            Assert.AreEqual(expected, examples[0]);
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\UnderholdningPage.html")]
        public void ParseExamples_ReturnsConcatenatedExamples_ForUnderholdningPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("UnderholdningPage.html"))
            {
                parser.LoadStream(ms);
            }

            List<string> examples = parser.ParseExamples();

            List<string> expected = new List<string>();
            expected.Add("1. 8000 medarbejdere skal til fest med god mad og underholdning af bl.a. Hans Otto Bisgaard.");
            expected.Add("2. vi havde jo ikke radio, TV eller video, så underholdningen bestod mest af kortspil i familien.");

            List<string> differences = expected.Except(examples).ToList();
            Assert.AreEqual(0, differences.Count, "Threre are differences between expected and actual lists with examples.");
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\KiggePage.html")]
        public void ParseExamples_ReturnsConcatenatedExamples_ForKiggePage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("KiggePage.html"))
            {
                parser.LoadStream(ms);
            }

            List<string> examples = parser.ParseExamples();

            List<string> expected = new List<string>();
            expected.Add("1. Børnene kiggede spørgende på hinanden.");
            expected.Add("2. kig lige en gang!");
            expected.Add("3. Han kiggede sig rundt, som om han ledte efter noget.");
            expected.Add("4. hun har kigget på de psykiske eftervirkninger hos voldtagne piger og kvinder.");
            expected.Add("5. Vi kigger efter en bil i det prislag, og Carinaen opfylder de fleste af de krav, vi stiller.");
            expected.Add("6. Berg er ikke altid lige smart, bl.a. ikke når hun afleverer blækregning for sent OG vedlægger den opgave, hun har kigget efter.");
            expected.Add("7. Har du lyst til at gå ud og kigge stjerner, Oskar? Det er sådan et smukt vejr.");

            List<string> differences = expected.Except(examples).ToList();
            Assert.AreEqual(0, differences.Count, "Threre are differences between expected and actual lists with examples.");
        }

        [TestMethod]
        [DeploymentItem(@"TestPages\ddo\GrillspydPage.html")]
        public void ParseExamples_ReturnsEmptyList_ForGrillspydPage()
        {
            DDOPageParser parser = new DDOPageParser();

            using (MemoryStream ms = Helpers.GetSimpleHTMLPageStream("GrillspydPage.html"))
            {
                parser.LoadStream(ms);
            }

            List<string> examples = parser.ParseExamples();

            Assert.AreEqual(0, examples.Count);
        }

        #endregion        
    }
}
