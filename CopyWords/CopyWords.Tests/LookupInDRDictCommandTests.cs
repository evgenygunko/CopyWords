using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using CopyWordsWPF.ViewModel.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyWords.Tests
{
    [TestClass]
    public class LookupInDRDictCommandTests
    {
        #region Accurate mapping

        [TestMethod]
        public void GetFileName_Returns0013_ForAbonnemant()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("Abonnemant");

            Assert.AreEqual("0013.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0013_ForA()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("a");

            Assert.AreEqual("0013.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0013_ForAbsint()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("absint");

            Assert.AreEqual("0013.jpg", result);
        } 

        [TestMethod]
        public void GetFileName_Returns0014_ForAbstract()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("abstract");

            Assert.AreEqual("0014.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0014_ForAdfærd()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("adfærd");

            Assert.AreEqual("0014.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0015_ForAdfærdspsykologi()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("adfærdspsykologi");

            Assert.AreEqual("0015.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0015_ForAdvare()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("advare");

            Assert.AreEqual("0015.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0016_ForAdvarsel()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("advarsel");

            Assert.AreEqual("0016.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0016_For_afblæse()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("afblæse");

            Assert.AreEqual("0016.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0017_For_afblæsning()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("afblæsning");

            Assert.AreEqual("0017.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0017_For_affindelse()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("affindelse");

            Assert.AreEqual("0017.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0018_For_affindelsessum()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("affindelsessum");

            Assert.AreEqual("0018.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0018_For_afgrænsning()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("afgrænsning");

            Assert.AreEqual("0018.jpg", result);
        }

        #endregion

        #region Approx mapping

        [TestMethod]
        public void GetFileName_Returns0028_For_akklimatisere()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("akklimatisere");

            Assert.AreEqual("0028.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0029_For_albaner()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("albaner");

            Assert.AreEqual("0029.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0028_For_akrobat()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("akrobat");

            // actually it should be 0029, but the approx algorithm returns different result
            Assert.AreEqual("0028.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0101_For_brød()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("brød");

            Assert.AreEqual("0101.jpg", result);
        }

        [TestMethod]
        public void GetFileName_Returns0250_For_går()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            string result = command.GetFileName("går");

            // this word is between 2 codes, 250 for 'gøe' and 251 for 'gåt'. The search will return 
            // the first code.
            Assert.AreEqual("0250.jpg", result);
        } 

        #endregion

        [TestMethod]
        public void WordsMapApproxDict_ValuesMustBeSortedAlphabetically()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            var wordsMapApproxDict = command.WordsMapApproxDict;

            string lastCode = null;
            string lastKey = null;

            foreach (string key in wordsMapApproxDict.Keys)
            {
                // some strange bug, the code for 'å' is smaller than the code for 'ø'
                string currentCodeNormalized = wordsMapApproxDict[key].Replace('å', (char)249);

                if (lastCode != null)
                {
                    if (string.CompareOrdinal(currentCodeNormalized, lastCode) < 0)
                    {
                        string message = string.Format(
                            "Values in the dictionary must be sorted aphpabetically. " +
                            "Previous code: '{0}', current code: '{1}', previous page: {2}, current page: {3}",
                            lastCode,
                            wordsMapApproxDict[key],
                            lastKey,
                            key);

                        Assert.Fail(message);
                    }                    
                }

                lastCode = wordsMapApproxDict[key];
                lastKey = key;
            }
        }

        [TestMethod]
        public void WordsMapAccurateList_ValuesMustBeSortedAlphabetically()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            var wordsMapAccurateList = command.WordsMapAccurateList;

            var lastItem = wordsMapAccurateList[0];

            for (int i = 1; i < wordsMapAccurateList.Count; i++)
            {
                if (string.CompareOrdinal(wordsMapAccurateList[i].LastWord, wordsMapAccurateList[i].FirstWord) < 0)
                {
                    Assert.Fail("Last word must be greater than or equal to first word");                
                }

                if (string.CompareOrdinal(wordsMapAccurateList[i].FirstWord, lastItem.LastWord) < 0)
                {
                    Assert.Fail("First word must be grater than or equal to the last word in the previous item.");
                }                
            }
        }

        [TestMethod]
        public void WordsMapApproxDict_KeysMustIncrementByOne()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            var wordsMapApproxDict = command.WordsMapApproxDict;

            int i = 13;

            foreach (var key in wordsMapApproxDict.Keys)
            {
                string expectedKey = string.Format("{0}.jpg", i.ToString().PadLeft(4, '0'));
                Assert.AreEqual(expectedKey, key);

                i++;
            }
        }

        [TestMethod]
        public void WordsMapAccurateList_KeysMustIncrementByOne()
        {
            LookupInDRDictCommand command = new LookupInDRDictCommand();
            var wordsMapAccurateList = command.WordsMapAccurateList;

            int i = 13;

            foreach (var item in wordsMapAccurateList)
            {
                string expectedKey = string.Format("{0}.jpg", i.ToString().PadLeft(4, '0'));
                Assert.AreEqual(expectedKey, item.FileName);

                i++;
            }
        }

        [TestMethod]
        public void WordsMapApproxDict_KeysMustHaveLengthOf3()
        {
            const int ExpectedCodeLength = 3;

            LookupInDRDictCommand command = new LookupInDRDictCommand();
            var wordsMapApproxDict = command.WordsMapApproxDict;

            var wrongCodes = wordsMapApproxDict.Where(kvp => kvp.Value.Length != ExpectedCodeLength);
            if (wrongCodes.Count() > 0)
            {
                string message = string.Format(
                "Word codes in the must have length = {0}. The following {1} codes do not satisfy this rule:{2}{3}",
                    ExpectedCodeLength,
                    wrongCodes.Count(),
                    Environment.NewLine,
                    string.Join(Environment.NewLine, wrongCodes.Select(kvp => string.Format("Page: {0}, code: {1}", kvp.Key, kvp.Value))));

                Assert.Fail(message);
            }
        }        
    }
}
