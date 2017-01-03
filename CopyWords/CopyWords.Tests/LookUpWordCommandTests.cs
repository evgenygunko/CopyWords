using CopyWordsWPF.ViewModel.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyWords.Tests
{
    [TestClass]
    public class LookUpWordCommandTests
    {
        [TestMethod]
        public void GetWordToLookupFromUrl_ReturnsKigge()
        {
            string result = LookUpWordCommand.GetWordToLookupFromUrl(@"http://ordnet.dk/ddo/ordbog?query=kigge&search=S%C3%B8g");
            Assert.AreEqual("kigge", result);
        }

        [TestMethod]
        public void GetWordToLookupFromUrl_ReturnsRefusionsopgørelse()
        {
            string result = LookUpWordCommand.GetWordToLookupFromUrl(@"http://ordnet.dk/ddo/ordbog?query=Refusionsopg%C3%B8relse&search=S%C3%B8g");
            Assert.AreEqual("refusionsopgørelse", result);
        }

        [TestMethod]
        public void GetSlovardkUri_ReturnsUriForKigge()
        {
            string result = LookUpWordCommand.GetSlovardkUri("kigge");
            Assert.AreEqual("http://www.slovar.dk/tdansk/kigge/?", result);
        }

        [TestMethod]
        public void GetSlovardkUri_ReturnsUriForAfgørelse()
        {
            string result = LookUpWordCommand.GetSlovardkUri("afgørelse");
            Assert.AreEqual("http://www.slovar.dk/tdansk/afg'oerelse/?", result);
        }

        [TestMethod]
        public void GetSlovardkUri_ReturnsUriForÅl()
        {
            string result = LookUpWordCommand.GetSlovardkUri("ål");
            Assert.AreEqual("http://www.slovar.dk/tdansk/'aal/?", result);
        }

        [TestMethod]
        public void GetSlovardkUri_ReturnsUriForKæmpe()
        {
            string result = LookUpWordCommand.GetSlovardkUri("Kæmpe");
            Assert.AreEqual("http://www.slovar.dk/tdansk/k'aempe/?", result);
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsFalse_ForUrl()
        {
            Assert.IsFalse(LookUpWordCommand.CheckThatWordIsValid(@"http://ordnet.dk/ddo/ordbog"));
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsFalse_ForQuote()
        {
            Assert.IsFalse(LookUpWordCommand.CheckThatWordIsValid(@"ordbo'g"));
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsTrue_ForWord()
        {
            Assert.IsTrue(LookUpWordCommand.CheckThatWordIsValid(@"refusionsopgørelse"));
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsTrue_ForTwoWord()
        {
            Assert.IsTrue(LookUpWordCommand.CheckThatWordIsValid(@"rindende vand"));
        }
    }
}
