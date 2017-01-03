using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyWords.Tests
{
    public class Helpers
    {
        public static MemoryStream GetSimpleHTMLPageStream(string fileName)
        {
            return GetSimpleHTMLPageStream(fileName, System.Text.Encoding.UTF8);
        }

        public static MemoryStream GetSimpleHTMLPageStream(string fileName, System.Text.Encoding encoding)
        {
            string fileContent;
            using (StreamReader sr = new StreamReader(fileName, encoding))
            {
                fileContent = sr.ReadToEnd();
            }

            Assert.IsFalse(
                string.IsNullOrEmpty(fileContent),
                string.Format(CultureInfo.InvariantCulture, "Cannot read content of {0} file.", fileName));

            return new MemoryStream(encoding.GetBytes(fileContent));
        }
    }
}
