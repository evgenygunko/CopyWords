using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyWordsWPF.Parsers
{
    public class RussianTranslation
    {
        public string DanishWord { get; private set; }

        public string Translation { get; private set; }

        public RussianTranslation(string danishWord, string translation)
        {
            DanishWord = danishWord;
            Translation = translation;
        }
    }
}
