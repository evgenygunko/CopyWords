namespace CopyWordsWPF.Parsers
{
    public class RussianTranslation
    {
        public RussianTranslation(string danishWord, string translation)
        {
            DanishWord = danishWord;
            Translation = translation;
        }

        public string DanishWord { get; private set; }

        public string Translation { get; private set; }
    }
}
