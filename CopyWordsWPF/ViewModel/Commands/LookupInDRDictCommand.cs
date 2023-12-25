using System.IO;
using System.Windows;
using CopyWordsWPF.Services;
using CopyWordsWPF.View;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class LookupInDRDictCommand : CommandBase
    {
        private readonly ISettingsService _settingsService;

        private List<WordToPageMap> _wordsMapAccurateList;
        private Dictionary<string, string> _wordsMapApproxDict = new Dictionary<string, string>();

        public LookupInDRDictCommand(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            FillThePreciseMap();
            FillTheApproxMap();
        }

        public override void Execute(object parameter)
        {
            if (!Directory.Exists(_settingsService.GetDanRusDictionaryFolder()))
            {
                string message = string.Format(
                    "Directory for Danish-Russian dictionary specified " +
                    "in settings does not exist '{0}'. Please open Settings dialog and select a valid directory.",
                    _settingsService.GetDanRusDictionaryFolder());

                MessageBox.Show(
                    message,
                    "Cannot find directory for Danish-Russian dictionary",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            string word = parameter as string;
            string fileName = GetFileName(word);

            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show(
                    string.Format("Cannot find a scanned page for the word '{0}'", word),
                    "Cannot find scanned file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            List<string> allPages = new List<string>();
            for (int i = 13, j = 0; j < _wordsMapApproxDict.Count; i++, j++)
            {
                allPages.Add(string.Format("{0}.jpg", i.ToString().PadLeft(4, '0')));
            }

            string title = string.Format("Word: '{0}', image file: '{1}'", word, fileName);
            ScannedImageViewModel vm = new ScannedImageViewModel(_settingsService, title, allPages, fileName);

            ScannedImageWindow w = new ScannedImageWindow()
            {
                DataContext = vm
            };

            w.Show();
        }

        internal string GetFileName(string word)
        {
            // Lookup in the list with accurate mapping first
            foreach (WordToPageMap wordToPageMap in _wordsMapAccurateList)
            {
                if (string.Compare(wordToPageMap.FirstWord, word, true) <= 0 &&
                    string.Compare(wordToPageMap.LastWord, word, true) >= 0)
                {
                    return wordToPageMap.FileName;
                }
            }

            // didn't find anything, lookup in the less accurate list
            string key = string.Empty;
            int lastComparison = 0;

            foreach (KeyValuePair<string, string> item in _wordsMapApproxDict)
            {
                if (word.StartsWith(item.Value))
                {
                    // exact match, returning current key
                    key = item.Key;
                    break;
                }

                // some strange bug, the code for 'å' (229) is smaller than the code for 'ø' (248)
                string currentCodeNormalized = item.Value.Replace('å', (char)249);
                string wordNormalized = word.Replace('å', (char)249);

                int currComparison = string.CompareOrdinal(currentCodeNormalized, wordNormalized);

                if (currComparison == 0)
                {
                    // exact match, returning current key
                    key = item.Key;
                    break;
                }
                else if (currComparison > 0 && lastComparison < 0)
                {
                    // will use the key from previous word
                    break;
                }

                key = item.Key;
                lastComparison = currComparison;
            }

            return key;
        }

        #region Fill the precise map

        private void FillThePreciseMap()
        {
            // the would be too tedious and big job to fill all pages with fist and end words,
            // so we will use approximation, see FillTheApproxMap() method.
            _wordsMapAccurateList = new List<WordToPageMap>()
            {
                new WordToPageMap("0013.jpg", "a", "absint"),
                new WordToPageMap("0014.jpg", "absolut", "adfærd"),
                new WordToPageMap("0015.jpg", "adfærdspsykologi", "advare"),
                new WordToPageMap("0016.jpg", "advarsel", "afblæse"),
                new WordToPageMap("0017.jpg", "afblæsning", "affindelse"),
                new WordToPageMap("0018.jpg", "affindelsessum", "afgrænsning"),
                new WordToPageMap("0019.jpg", "afgræsning", "afhåre"),
                new WordToPageMap("0020.jpg", "afilte", "aflire"),
                new WordToPageMap("0021.jpg", "aflive", "aforisme"),
                new WordToPageMap("0022.jpg", "afparere", "afsindig"),
                new WordToPageMap("0023.jpg", "afsjælet", "afslibe"),
                new WordToPageMap("0024.jpg", "afslutning", "afsted"),
                new WordToPageMap("0025.jpg", "afstedkomme", "aftale"),
                new WordToPageMap("0026.jpg", "aftalt", "afventende"),
                new WordToPageMap("0027.jpg", "afvige", "agronomi"),

                //new WordToPageMap("0028.jpg", "", ""),
                //new WordToPageMap("0029.jpg", "", ""),
                //new WordToPageMap("0030.jpg", "", ""),
                //new WordToPageMap("0031.jpg", "", ""),
                //new WordToPageMap("0032.jpg", "", ""),
                //new WordToPageMap("0033.jpg", "", ""),
                //new WordToPageMap("0034.jpg", "", ""),
                //new WordToPageMap("0035.jpg", "", ""),
                //new WordToPageMap("0036.jpg", "", ""),
                //new WordToPageMap("0037.jpg", "", ""),
                //new WordToPageMap("0038.jpg", "", ""),
                //new WordToPageMap("0039.jpg", "", ""),
                //new WordToPageMap("0040.jpg", "", ""),
                //new WordToPageMap("0041.jpg", "", ""),
                //new WordToPageMap("0042.jpg", "", ""),
                //new WordToPageMap("0043.jpg", "", ""),
                //new WordToPageMap("0044.jpg", "", ""),
                //new WordToPageMap("0045.jpg", "", ""),
                //new WordToPageMap("0046.jpg", "", ""),
                //new WordToPageMap("0047.jpg", "", ""),
                //new WordToPageMap("0048.jpg", "", ""),
                //new WordToPageMap("0049.jpg", "", ""),
                //new WordToPageMap("0050.jpg", "", ""),
                //new WordToPageMap("0051.jpg", "", ""),
                //new WordToPageMap("0052.jpg", "", ""),
                //new WordToPageMap("0053.jpg", "", ""),
                //new WordToPageMap("0054.jpg", "", ""),
                //new WordToPageMap("0055.jpg", "", ""),
                //new WordToPageMap("0056.jpg", "", ""),
                //new WordToPageMap("0057.jpg", "", ""),
                //new WordToPageMap("0058.jpg", "", ""),
                //new WordToPageMap("0059.jpg", "", ""),
                //new WordToPageMap("0060.jpg", "", ""),
                //new WordToPageMap("0061.jpg", "", ""),
                //new WordToPageMap("0062.jpg", "", ""),
                //new WordToPageMap("0063.jpg", "", ""),
                //new WordToPageMap("0064.jpg", "", ""),
                //new WordToPageMap("0065.jpg", "", ""),
                //new WordToPageMap("0066.jpg", "", ""),
                //new WordToPageMap("0067.jpg", "", ""),
                //new WordToPageMap("0068.jpg", "", ""),
                //new WordToPageMap("0069.jpg", "", ""),
                //new WordToPageMap("0070.jpg", "", ""),
                //new WordToPageMap("0071.jpg", "", ""),
                //new WordToPageMap("0072.jpg", "", ""),
                //new WordToPageMap("0073.jpg", "", ""),
                //new WordToPageMap("0074.jpg", "", ""),
                //new WordToPageMap("0075.jpg", "", ""),
                //new WordToPageMap("0076.jpg", "", ""),
                //new WordToPageMap("0077.jpg", "", ""),
                //new WordToPageMap("0078.jpg", "", ""),
                //new WordToPageMap("0079.jpg", "", ""),
                //new WordToPageMap("0080.jpg", "", ""),
                //new WordToPageMap("0081.jpg", "", ""),
                //new WordToPageMap("0082.jpg", "", ""),
                //new WordToPageMap("0083.jpg", "", ""),
                //new WordToPageMap("0084.jpg", "", ""),
                //new WordToPageMap("0085.jpg", "", ""),
                //new WordToPageMap("0086.jpg", "", ""),
                //new WordToPageMap("0087.jpg", "", ""),
                //new WordToPageMap("0088.jpg", "", ""),
                //new WordToPageMap("0089.jpg", "", ""),
                //new WordToPageMap("0090.jpg", "", ""),
                //new WordToPageMap("0091.jpg", "", ""),
                //new WordToPageMap("0092.jpg", "", ""),
                //new WordToPageMap("0093.jpg", "", ""),
                //new WordToPageMap("0094.jpg", "", ""),
                //new WordToPageMap("0095.jpg", "", ""),
                //new WordToPageMap("0096.jpg", "", ""),
                //new WordToPageMap("0097.jpg", "", ""),
                //new WordToPageMap("0098.jpg", "", ""),
                //new WordToPageMap("0099.jpg", "", ""),
                //new WordToPageMap("0100.jpg", "", ""),
                //new WordToPageMap("0101.jpg", "", ""),
                //new WordToPageMap("0102.jpg", "", ""),
                //new WordToPageMap("0103.jpg", "", ""),
                //new WordToPageMap("0104.jpg", "", ""),
                //new WordToPageMap("0105.jpg", "", ""),
                //new WordToPageMap("0106.jpg", "", ""),
                //new WordToPageMap("0107.jpg", "", ""),
                //new WordToPageMap("0108.jpg", "", ""),
                //new WordToPageMap("0109.jpg", "", ""),
                //new WordToPageMap("0110.jpg", "", ""),
                //new WordToPageMap("0111.jpg", "", ""),
                //new WordToPageMap("0112.jpg", "", ""),
                //new WordToPageMap("0113.jpg", "", ""),
                //new WordToPageMap("0114.jpg", "", ""),
                //new WordToPageMap("0115.jpg", "", ""),
                //new WordToPageMap("0116.jpg", "", ""),
                //new WordToPageMap("0117.jpg", "", ""),
                //new WordToPageMap("0118.jpg", "", ""),
                //new WordToPageMap("0119.jpg", "", ""),
                //new WordToPageMap("0120.jpg", "", ""),
                //new WordToPageMap("0121.jpg", "", ""),
                //new WordToPageMap("0122.jpg", "", ""),
                //new WordToPageMap("0123.jpg", "", ""),
                //new WordToPageMap("0124.jpg", "", ""),
                //new WordToPageMap("0125.jpg", "", ""),
                //new WordToPageMap("0126.jpg", "", ""),
                //new WordToPageMap("0127.jpg", "", ""),
                //new WordToPageMap("0128.jpg", "", ""),
                //new WordToPageMap("0129.jpg", "", ""),
                //new WordToPageMap("0130.jpg", "", ""),
                //new WordToPageMap("0131.jpg", "", ""),
                //new WordToPageMap("0132.jpg", "", ""),
                //new WordToPageMap("0133.jpg", "", ""),
                //new WordToPageMap("0134.jpg", "", ""),
                //new WordToPageMap("0135.jpg", "", ""),
                //new WordToPageMap("0136.jpg", "", ""),
                //new WordToPageMap("0137.jpg", "", ""),
                //new WordToPageMap("0138.jpg", "", ""),
                //new WordToPageMap("0139.jpg", "", ""),
                //new WordToPageMap("0140.jpg", "", ""),
                //new WordToPageMap("0141.jpg", "", ""),
                //new WordToPageMap("0142.jpg", "", ""),
                //new WordToPageMap("0143.jpg", "", ""),
                //new WordToPageMap("0144.jpg", "", ""),
                //new WordToPageMap("0145.jpg", "", ""),
                //new WordToPageMap("0146.jpg", "", ""),
                //new WordToPageMap("0147.jpg", "", ""),
                //new WordToPageMap("0148.jpg", "", ""),
                //new WordToPageMap("0149.jpg", "", ""),
                //new WordToPageMap("0150.jpg", "", ""),
                //new WordToPageMap("0151.jpg", "", ""),
                //new WordToPageMap("0152.jpg", "", ""),
                //new WordToPageMap("0153.jpg", "", ""),
                //new WordToPageMap("0154.jpg", "", ""),
                //new WordToPageMap("0155.jpg", "", ""),
                //new WordToPageMap("0156.jpg", "", ""),
                //new WordToPageMap("0157.jpg", "", ""),
                //new WordToPageMap("0158.jpg", "", ""),
                //new WordToPageMap("0159.jpg", "", ""),
                //new WordToPageMap("0160.jpg", "", ""),
                //new WordToPageMap("0161.jpg", "", ""),
                //new WordToPageMap("0162.jpg", "", ""),
                //new WordToPageMap("0163.jpg", "", ""),
                //new WordToPageMap("0164.jpg", "", ""),
                //new WordToPageMap("0165.jpg", "", ""),
                //new WordToPageMap("0166.jpg", "", ""),
                //new WordToPageMap("0167.jpg", "", ""),
                //new WordToPageMap("0168.jpg", "", ""),
                //new WordToPageMap("0169.jpg", "", ""),
                //new WordToPageMap("0170.jpg", "", ""),
                //new WordToPageMap("0171.jpg", "", ""),
                //new WordToPageMap("0172.jpg", "", ""),
                //new WordToPageMap("0173.jpg", "", ""),
                //new WordToPageMap("0174.jpg", "", ""),
                //new WordToPageMap("0175.jpg", "", ""),
                //new WordToPageMap("0176.jpg", "", ""),
                //new WordToPageMap("0177.jpg", "", ""),
                //new WordToPageMap("0178.jpg", "", ""),
                //new WordToPageMap("0179.jpg", "", ""),
                //new WordToPageMap("0180.jpg", "", ""),
                //new WordToPageMap("0181.jpg", "", ""),
                //new WordToPageMap("0182.jpg", "", ""),
                //new WordToPageMap("0183.jpg", "", ""),
                //new WordToPageMap("0184.jpg", "", ""),
                //new WordToPageMap("0185.jpg", "", ""),
                //new WordToPageMap("0186.jpg", "", ""),
                //new WordToPageMap("0187.jpg", "", ""),
                //new WordToPageMap("0188.jpg", "", ""),
                //new WordToPageMap("0189.jpg", "", ""),
                //new WordToPageMap("0190.jpg", "", ""),
                //new WordToPageMap("0191.jpg", "", ""),
                //new WordToPageMap("0192.jpg", "", ""),
                //new WordToPageMap("0193.jpg", "", ""),
                //new WordToPageMap("0194.jpg", "", ""),
                //new WordToPageMap("0195.jpg", "", ""),
                //new WordToPageMap("0196.jpg", "", ""),
                //new WordToPageMap("0197.jpg", "", ""),
                //new WordToPageMap("0198.jpg", "", ""),
                //new WordToPageMap("0199.jpg", "", ""),
                //new WordToPageMap("0200.jpg", "", ""),
                //new WordToPageMap("0201.jpg", "", ""),
                //new WordToPageMap("0202.jpg", "", ""),
                //new WordToPageMap("0203.jpg", "", ""),
                //new WordToPageMap("0204.jpg", "", ""),
                //new WordToPageMap("0205.jpg", "", ""),
                //new WordToPageMap("0206.jpg", "", ""),
                //new WordToPageMap("0207.jpg", "", ""),
                //new WordToPageMap("0208.jpg", "", ""),
                //new WordToPageMap("0209.jpg", "", ""),
                //new WordToPageMap("0210.jpg", "", ""),
                //new WordToPageMap("0211.jpg", "", ""),
                //new WordToPageMap("0212.jpg", "", ""),
                //new WordToPageMap("0213.jpg", "", ""),
                //new WordToPageMap("0214.jpg", "", ""),
                //new WordToPageMap("0215.jpg", "", ""),
                //new WordToPageMap("0216.jpg", "", ""),
                //new WordToPageMap("0217.jpg", "", ""),
                //new WordToPageMap("0218.jpg", "", ""),
                //new WordToPageMap("0219.jpg", "", ""),
                //new WordToPageMap("0220.jpg", "", ""),
                //new WordToPageMap("0221.jpg", "", ""),
                //new WordToPageMap("0222.jpg", "", ""),
                //new WordToPageMap("0223.jpg", "", ""),
                //new WordToPageMap("0224.jpg", "", ""),
                //new WordToPageMap("0225.jpg", "", ""),
                //new WordToPageMap("0226.jpg", "", ""),
                //new WordToPageMap("0227.jpg", "", ""),
                //new WordToPageMap("0228.jpg", "", ""),
                //new WordToPageMap("0229.jpg", "", ""),
                //new WordToPageMap("0230.jpg", "", ""),
                //new WordToPageMap("0231.jpg", "", ""),
                //new WordToPageMap("0232.jpg", "", ""),
                //new WordToPageMap("0233.jpg", "", ""),
                //new WordToPageMap("0234.jpg", "", ""),
                //new WordToPageMap("0235.jpg", "", ""),
                //new WordToPageMap("0236.jpg", "", ""),
                //new WordToPageMap("0237.jpg", "", ""),
                //new WordToPageMap("0238.jpg", "", ""),
                //new WordToPageMap("0239.jpg", "", ""),
                //new WordToPageMap("0240.jpg", "", ""),
                //new WordToPageMap("0241.jpg", "", ""),
                //new WordToPageMap("0242.jpg", "", ""),
                //new WordToPageMap("0243.jpg", "", ""),
                //new WordToPageMap("0244.jpg", "", ""),
                //new WordToPageMap("0245.jpg", "", ""),
                //new WordToPageMap("0246.jpg", "", ""),
                //new WordToPageMap("0247.jpg", "", ""),
                //new WordToPageMap("0248.jpg", "", ""),
                //new WordToPageMap("0249.jpg", "", ""),
                //new WordToPageMap("0250.jpg", "", ""),
                //new WordToPageMap("0251.jpg", "", ""),
                //new WordToPageMap("0252.jpg", "", ""),
                //new WordToPageMap("0253.jpg", "", ""),
                //new WordToPageMap("0254.jpg", "", ""),
                //new WordToPageMap("0255.jpg", "", ""),
                //new WordToPageMap("0256.jpg", "", ""),
                //new WordToPageMap("0257.jpg", "", ""),
                //new WordToPageMap("0258.jpg", "", ""),
                //new WordToPageMap("0259.jpg", "", ""),
                //new WordToPageMap("0260.jpg", "", ""),
                //new WordToPageMap("0261.jpg", "", ""),
                //new WordToPageMap("0262.jpg", "", ""),
                //new WordToPageMap("0263.jpg", "", ""),
                //new WordToPageMap("0264.jpg", "", ""),
                //new WordToPageMap("0265.jpg", "", ""),
                //new WordToPageMap("0266.jpg", "", ""),
                //new WordToPageMap("0267.jpg", "", ""),
                //new WordToPageMap("0268.jpg", "", ""),
                //new WordToPageMap("0269.jpg", "", ""),
                //new WordToPageMap("0270.jpg", "", ""),
                //new WordToPageMap("0271.jpg", "", ""),
                //new WordToPageMap("0272.jpg", "", ""),
                //new WordToPageMap("0273.jpg", "", ""),
                //new WordToPageMap("0274.jpg", "", ""),
                //new WordToPageMap("0275.jpg", "", ""),
                //new WordToPageMap("0276.jpg", "", ""),
                //new WordToPageMap("0277.jpg", "", ""),
                //new WordToPageMap("0278.jpg", "", ""),
                //new WordToPageMap("0279.jpg", "", ""),
                //new WordToPageMap("0280.jpg", "", ""),
                //new WordToPageMap("0281.jpg", "", ""),
                //new WordToPageMap("0282.jpg", "", ""),
                //new WordToPageMap("0283.jpg", "", ""),
                //new WordToPageMap("0284.jpg", "", ""),
                //new WordToPageMap("0285.jpg", "", ""),
                //new WordToPageMap("0286.jpg", "", ""),
                //new WordToPageMap("0287.jpg", "", ""),
                //new WordToPageMap("0288.jpg", "", ""),
                //new WordToPageMap("0289.jpg", "", ""),
                //new WordToPageMap("0290.jpg", "", ""),
                //new WordToPageMap("0291.jpg", "", ""),
                //new WordToPageMap("0292.jpg", "", ""),
                //new WordToPageMap("0293.jpg", "", ""),
                //new WordToPageMap("0294.jpg", "", ""),
                //new WordToPageMap("0295.jpg", "", ""),
                //new WordToPageMap("0296.jpg", "", ""),
                //new WordToPageMap("0297.jpg", "", ""),
                //new WordToPageMap("0298.jpg", "", ""),
                //new WordToPageMap("0299.jpg", "", ""),
                //new WordToPageMap("0300.jpg", "", ""),
                //new WordToPageMap("0301.jpg", "", ""),
                //new WordToPageMap("0302.jpg", "", ""),
                //new WordToPageMap("0303.jpg", "", ""),
                //new WordToPageMap("0304.jpg", "", ""),
                //new WordToPageMap("0305.jpg", "", ""),
                //new WordToPageMap("0306.jpg", "", ""),
                //new WordToPageMap("0307.jpg", "", ""),
                //new WordToPageMap("0308.jpg", "", ""),
                //new WordToPageMap("0309.jpg", "", ""),
                //new WordToPageMap("0310.jpg", "", ""),
                //new WordToPageMap("0311.jpg", "", ""),
                //new WordToPageMap("0312.jpg", "", ""),
                //new WordToPageMap("0313.jpg", "", ""),
                //new WordToPageMap("0314.jpg", "", ""),
                //new WordToPageMap("0315.jpg", "", ""),
                //new WordToPageMap("0316.jpg", "", ""),
                //new WordToPageMap("0317.jpg", "", ""),
                //new WordToPageMap("0318.jpg", "", ""),
                //new WordToPageMap("0319.jpg", "", ""),
                //new WordToPageMap("0320.jpg", "", ""),
                //new WordToPageMap("0321.jpg", "", ""),
                //new WordToPageMap("0322.jpg", "", ""),
                //new WordToPageMap("0323.jpg", "", ""),
                //new WordToPageMap("0324.jpg", "", ""),
                //new WordToPageMap("0325.jpg", "", ""),
                //new WordToPageMap("0326.jpg", "", ""),
                //new WordToPageMap("0327.jpg", "", ""),
                //new WordToPageMap("0328.jpg", "", ""),
                //new WordToPageMap("0329.jpg", "", ""),
                //new WordToPageMap("0330.jpg", "", ""),
                //new WordToPageMap("0331.jpg", "", ""),
                //new WordToPageMap("0332.jpg", "", ""),
                //new WordToPageMap("0333.jpg", "", ""),
                //new WordToPageMap("0334.jpg", "", ""),
                //new WordToPageMap("0335.jpg", "", ""),
                //new WordToPageMap("0336.jpg", "", ""),
                //new WordToPageMap("0337.jpg", "", ""),
                //new WordToPageMap("0338.jpg", "", ""),
                //new WordToPageMap("0339.jpg", "", ""),
                //new WordToPageMap("0340.jpg", "", ""),
                //new WordToPageMap("0341.jpg", "", ""),
                //new WordToPageMap("0342.jpg", "", ""),
                //new WordToPageMap("0343.jpg", "", ""),
                //new WordToPageMap("0344.jpg", "", ""),
                //new WordToPageMap("0345.jpg", "", ""),
                //new WordToPageMap("0346.jpg", "", ""),
                //new WordToPageMap("0347.jpg", "", ""),
                //new WordToPageMap("0348.jpg", "", ""),
                //new WordToPageMap("0349.jpg", "", ""),
                //new WordToPageMap("0350.jpg", "", ""),
                //new WordToPageMap("0351.jpg", "", ""),
                //new WordToPageMap("0352.jpg", "", ""),
                //new WordToPageMap("0353.jpg", "", ""),
                //new WordToPageMap("0354.jpg", "", ""),
                //new WordToPageMap("0355.jpg", "", ""),
                //new WordToPageMap("0356.jpg", "", ""),
                //new WordToPageMap("0357.jpg", "", ""),
                //new WordToPageMap("0358.jpg", "", ""),
                //new WordToPageMap("0359.jpg", "", ""),
                //new WordToPageMap("0360.jpg", "", ""),
                //new WordToPageMap("0361.jpg", "", ""),
                //new WordToPageMap("0362.jpg", "", ""),
                //new WordToPageMap("0363.jpg", "", ""),
                //new WordToPageMap("0364.jpg", "", ""),
                //new WordToPageMap("0365.jpg", "", ""),
                //new WordToPageMap("0366.jpg", "", ""),
                //new WordToPageMap("0367.jpg", "", ""),
                //new WordToPageMap("0368.jpg", "", ""),
                //new WordToPageMap("0369.jpg", "", ""),
                //new WordToPageMap("0370.jpg", "", ""),
                //new WordToPageMap("0371.jpg", "", ""),
                //new WordToPageMap("0372.jpg", "", ""),
                //new WordToPageMap("0373.jpg", "", ""),
                //new WordToPageMap("0374.jpg", "", ""),
                //new WordToPageMap("0375.jpg", "", ""),
                //new WordToPageMap("0376.jpg", "", ""),
                //new WordToPageMap("0377.jpg", "", ""),
                //new WordToPageMap("0378.jpg", "", ""),
                //new WordToPageMap("0379.jpg", "", ""),
                //new WordToPageMap("0380.jpg", "", ""),
                //new WordToPageMap("0381.jpg", "", ""),
                //new WordToPageMap("0382.jpg", "", ""),
                //new WordToPageMap("0383.jpg", "", ""),
                //new WordToPageMap("0384.jpg", "", ""),
                //new WordToPageMap("0385.jpg", "", ""),
                //new WordToPageMap("0386.jpg", "", ""),
                //new WordToPageMap("0387.jpg", "", ""),
                //new WordToPageMap("0388.jpg", "", ""),
                //new WordToPageMap("0389.jpg", "", ""),
                //new WordToPageMap("0390.jpg", "", ""),
                //new WordToPageMap("0391.jpg", "", ""),
                //new WordToPageMap("0392.jpg", "", ""),
                //new WordToPageMap("0393.jpg", "", ""),
                //new WordToPageMap("0394.jpg", "", ""),
                //new WordToPageMap("0395.jpg", "", ""),
                //new WordToPageMap("0396.jpg", "", ""),
                //new WordToPageMap("0397.jpg", "", ""),
                //new WordToPageMap("0398.jpg", "", ""),
                //new WordToPageMap("0399.jpg", "", ""),
                //new WordToPageMap("0400.jpg", "", ""),
                //new WordToPageMap("0401.jpg", "", ""),
                //new WordToPageMap("0402.jpg", "", ""),
                //new WordToPageMap("0403.jpg", "", ""),
                //new WordToPageMap("0404.jpg", "", ""),
                //new WordToPageMap("0405.jpg", "", ""),
                //new WordToPageMap("0406.jpg", "", ""),
                //new WordToPageMap("0407.jpg", "", ""),
                //new WordToPageMap("0408.jpg", "", ""),
                //new WordToPageMap("0409.jpg", "", ""),
                //new WordToPageMap("0410.jpg", "", ""),
                //new WordToPageMap("0411.jpg", "", ""),
                //new WordToPageMap("0412.jpg", "", ""),
                //new WordToPageMap("0413.jpg", "", ""),
                //new WordToPageMap("0414.jpg", "", ""),
                //new WordToPageMap("0415.jpg", "", ""),
                //new WordToPageMap("0416.jpg", "", ""),
                //new WordToPageMap("0417.jpg", "", ""),
                //new WordToPageMap("0418.jpg", "", ""),
                //new WordToPageMap("0419.jpg", "", ""),
                //new WordToPageMap("0420.jpg", "", ""),
                //new WordToPageMap("0421.jpg", "", ""),
                //new WordToPageMap("0422.jpg", "", ""),
                //new WordToPageMap("0423.jpg", "", ""),
                //new WordToPageMap("0424.jpg", "", ""),
                //new WordToPageMap("0425.jpg", "", ""),
                //new WordToPageMap("0426.jpg", "", ""),
                //new WordToPageMap("0427.jpg", "", ""),
                //new WordToPageMap("0428.jpg", "", ""),
                //new WordToPageMap("0429.jpg", "", ""),
                //new WordToPageMap("0430.jpg", "", ""),
                //new WordToPageMap("0431.jpg", "", ""),
                //new WordToPageMap("0432.jpg", "", ""),
                //new WordToPageMap("0433.jpg", "", ""),
                //new WordToPageMap("0434.jpg", "", ""),
                //new WordToPageMap("0435.jpg", "", ""),
                //new WordToPageMap("0436.jpg", "", ""),
                //new WordToPageMap("0437.jpg", "", ""),
                //new WordToPageMap("0438.jpg", "", ""),
                //new WordToPageMap("0439.jpg", "", ""),
                //new WordToPageMap("0440.jpg", "", ""),
                //new WordToPageMap("0441.jpg", "", ""),
                //new WordToPageMap("0442.jpg", "", ""),
                //new WordToPageMap("0443.jpg", "", ""),
                //new WordToPageMap("0444.jpg", "", ""),
                //new WordToPageMap("0445.jpg", "", ""),
                //new WordToPageMap("0446.jpg", "", ""),
                //new WordToPageMap("0447.jpg", "", ""),
                //new WordToPageMap("0448.jpg", "", ""),
                //new WordToPageMap("0449.jpg", "", ""),
                //new WordToPageMap("0450.jpg", "", ""),
                //new WordToPageMap("0451.jpg", "", ""),
                //new WordToPageMap("0452.jpg", "", ""),
                //new WordToPageMap("0453.jpg", "", ""),
                //new WordToPageMap("0454.jpg", "", ""),
                //new WordToPageMap("0455.jpg", "", ""),
                //new WordToPageMap("0456.jpg", "", ""),
                //new WordToPageMap("0457.jpg", "", ""),
                //new WordToPageMap("0458.jpg", "", ""),
                //new WordToPageMap("0459.jpg", "", ""),
                //new WordToPageMap("0460.jpg", "", ""),
                //new WordToPageMap("0461.jpg", "", ""),
                //new WordToPageMap("0462.jpg", "", ""),
                //new WordToPageMap("0463.jpg", "", ""),
                //new WordToPageMap("0464.jpg", "", ""),
                //new WordToPageMap("0465.jpg", "", ""),
                //new WordToPageMap("0466.jpg", "", ""),
                //new WordToPageMap("0467.jpg", "", ""),
                //new WordToPageMap("0468.jpg", "", ""),
                //new WordToPageMap("0469.jpg", "", ""),
                //new WordToPageMap("0470.jpg", "", ""),
                //new WordToPageMap("0471.jpg", "", ""),
                //new WordToPageMap("0472.jpg", "", ""),
                //new WordToPageMap("0473.jpg", "", ""),
                //new WordToPageMap("0474.jpg", "", ""),
                //new WordToPageMap("0475.jpg", "", ""),
                //new WordToPageMap("0476.jpg", "", ""),
                //new WordToPageMap("0477.jpg", "", ""),
                //new WordToPageMap("0478.jpg", "", ""),
                //new WordToPageMap("0479.jpg", "", ""),
                //new WordToPageMap("0480.jpg", "", ""),
                //new WordToPageMap("0481.jpg", "", ""),
                //new WordToPageMap("0482.jpg", "", ""),
                //new WordToPageMap("0483.jpg", "", ""),
                //new WordToPageMap("0484.jpg", "", ""),
                //new WordToPageMap("0485.jpg", "", ""),
                //new WordToPageMap("0486.jpg", "", ""),
                //new WordToPageMap("0487.jpg", "", ""),
                //new WordToPageMap("0488.jpg", "", ""),
                //new WordToPageMap("0489.jpg", "", ""),
                //new WordToPageMap("0490.jpg", "", ""),
                //new WordToPageMap("0491.jpg", "", ""),
                //new WordToPageMap("0492.jpg", "", ""),
                //new WordToPageMap("0493.jpg", "", ""),
                //new WordToPageMap("0494.jpg", "", ""),
                //new WordToPageMap("0495.jpg", "", ""),
                //new WordToPageMap("0496.jpg", "", ""),
                //new WordToPageMap("0497.jpg", "", ""),
                //new WordToPageMap("0498.jpg", "", ""),
                //new WordToPageMap("0499.jpg", "", ""),
                //new WordToPageMap("0500.jpg", "", ""),
                //new WordToPageMap("0501.jpg", "", ""),
                //new WordToPageMap("0502.jpg", "", ""),
                //new WordToPageMap("0503.jpg", "", ""),
                //new WordToPageMap("0504.jpg", "", ""),
                //new WordToPageMap("0505.jpg", "", ""),
                //new WordToPageMap("0506.jpg", "", ""),
                //new WordToPageMap("0507.jpg", "", ""),
                //new WordToPageMap("0508.jpg", "", ""),
                //new WordToPageMap("0509.jpg", "", ""),
                //new WordToPageMap("0510.jpg", "", ""),
                //new WordToPageMap("0511.jpg", "", ""),
                //new WordToPageMap("0512.jpg", "", ""),
                //new WordToPageMap("0513.jpg", "", ""),
                //new WordToPageMap("0514.jpg", "", ""),
                //new WordToPageMap("0515.jpg", "", ""),
                //new WordToPageMap("0516.jpg", "", ""),
                //new WordToPageMap("0517.jpg", "", ""),
                //new WordToPageMap("0518.jpg", "", ""),
                //new WordToPageMap("0519.jpg", "", ""),
                //new WordToPageMap("0520.jpg", "", ""),
                //new WordToPageMap("0521.jpg", "", ""),
                //new WordToPageMap("0522.jpg", "", ""),
                //new WordToPageMap("0523.jpg", "", ""),
                //new WordToPageMap("0524.jpg", "", ""),
                //new WordToPageMap("0525.jpg", "", ""),
                //new WordToPageMap("0526.jpg", "", ""),
                //new WordToPageMap("0527.jpg", "", ""),
                //new WordToPageMap("0528.jpg", "", ""),
                //new WordToPageMap("0529.jpg", "", ""),
                //new WordToPageMap("0530.jpg", "", ""),
                //new WordToPageMap("0531.jpg", "", ""),
                //new WordToPageMap("0532.jpg", "", ""),
                //new WordToPageMap("0533.jpg", "", ""),
                //new WordToPageMap("0534.jpg", "", ""),
                //new WordToPageMap("0535.jpg", "", ""),
                //new WordToPageMap("0536.jpg", "", ""),
                //new WordToPageMap("0537.jpg", "", ""),
                //new WordToPageMap("0538.jpg", "", ""),
                //new WordToPageMap("0539.jpg", "", ""),
                //new WordToPageMap("0540.jpg", "", ""),
                //new WordToPageMap("0541.jpg", "", ""),
                //new WordToPageMap("0542.jpg", "", ""),
                //new WordToPageMap("0543.jpg", "", ""),
                //new WordToPageMap("0544.jpg", "", ""),
                //new WordToPageMap("0545.jpg", "", ""),
                //new WordToPageMap("0546.jpg", "", ""),
                //new WordToPageMap("0547.jpg", "", ""),
                //new WordToPageMap("0548.jpg", "", ""),
                //new WordToPageMap("0549.jpg", "", ""),
                //new WordToPageMap("0550.jpg", "", ""),
                //new WordToPageMap("0551.jpg", "", ""),
                //new WordToPageMap("0552.jpg", "", ""),
                //new WordToPageMap("0553.jpg", "", ""),
                //new WordToPageMap("0554.jpg", "", ""),
                //new WordToPageMap("0555.jpg", "", ""),
                //new WordToPageMap("0556.jpg", "", ""),
                //new WordToPageMap("0557.jpg", "", ""),
                //new WordToPageMap("0558.jpg", "", ""),
                //new WordToPageMap("0559.jpg", "", ""),
                //new WordToPageMap("0560.jpg", "", ""),
                //new WordToPageMap("0561.jpg", "", ""),
                //new WordToPageMap("0562.jpg", "", ""),
                //new WordToPageMap("0563.jpg", "", ""),
                //new WordToPageMap("0564.jpg", "", ""),
                //new WordToPageMap("0565.jpg", "", ""),
                //new WordToPageMap("0566.jpg", "", ""),
                //new WordToPageMap("0567.jpg", "", ""),
                //new WordToPageMap("0568.jpg", "", ""),
                //new WordToPageMap("0569.jpg", "", ""),
                //new WordToPageMap("0570.jpg", "", ""),
                //new WordToPageMap("0571.jpg", "", ""),
                //new WordToPageMap("0572.jpg", "", ""),
                //new WordToPageMap("0573.jpg", "", ""),
                //new WordToPageMap("0574.jpg", "", ""),
                //new WordToPageMap("0575.jpg", "", ""),
                //new WordToPageMap("0576.jpg", "", ""),
                //new WordToPageMap("0577.jpg", "", ""),
                //new WordToPageMap("0578.jpg", "", ""),
                //new WordToPageMap("0579.jpg", "", ""),
                //new WordToPageMap("0580.jpg", "", ""),
                //new WordToPageMap("0581.jpg", "", ""),
                //new WordToPageMap("0582.jpg", "", ""),
                //new WordToPageMap("0583.jpg", "", ""),
                //new WordToPageMap("0584.jpg", "", ""),
                //new WordToPageMap("0585.jpg", "", ""),
                //new WordToPageMap("0586.jpg", "", ""),
                //new WordToPageMap("0587.jpg", "", ""),
                //new WordToPageMap("0588.jpg", "", ""),
                //new WordToPageMap("0589.jpg", "", ""),
                //new WordToPageMap("0590.jpg", "", ""),
                //new WordToPageMap("0591.jpg", "", ""),
                //new WordToPageMap("0592.jpg", "", ""),
                //new WordToPageMap("0593.jpg", "", ""),
                //new WordToPageMap("0594.jpg", "", ""),
                //new WordToPageMap("0595.jpg", "", ""),
                //new WordToPageMap("0596.jpg", "", ""),
                //new WordToPageMap("0597.jpg", "", ""),
                //new WordToPageMap("0598.jpg", "", ""),
                //new WordToPageMap("0599.jpg", "", ""),
                //new WordToPageMap("0600.jpg", "", ""),
                //new WordToPageMap("0601.jpg", "", ""),
                //new WordToPageMap("0602.jpg", "", ""),
                //new WordToPageMap("0603.jpg", "", ""),
                //new WordToPageMap("0604.jpg", "", ""),
                //new WordToPageMap("0605.jpg", "", ""),
                //new WordToPageMap("0606.jpg", "", ""),
                //new WordToPageMap("0607.jpg", "", ""),
                //new WordToPageMap("0608.jpg", "", ""),
                //new WordToPageMap("0609.jpg", "", ""),
                //new WordToPageMap("0610.jpg", "", ""),
                //new WordToPageMap("0611.jpg", "", ""),
                //new WordToPageMap("0612.jpg", "", ""),
                //new WordToPageMap("0613.jpg", "", ""),
                //new WordToPageMap("0614.jpg", "", ""),
                //new WordToPageMap("0615.jpg", "", ""),
                //new WordToPageMap("0616.jpg", "", ""),
                //new WordToPageMap("0617.jpg", "", ""),
                //new WordToPageMap("0618.jpg", "", ""),
                //new WordToPageMap("0619.jpg", "", ""),
                //new WordToPageMap("0620.jpg", "", ""),
                //new WordToPageMap("0621.jpg", "", ""),
                //new WordToPageMap("0622.jpg", "", ""),
                //new WordToPageMap("0623.jpg", "", ""),
                //new WordToPageMap("0624.jpg", "", ""),
                //new WordToPageMap("0625.jpg", "", ""),
                //new WordToPageMap("0626.jpg", "", ""),
                //new WordToPageMap("0627.jpg", "", ""),
                //new WordToPageMap("0628.jpg", "", ""),
                //new WordToPageMap("0629.jpg", "", ""),
                //new WordToPageMap("0630.jpg", "", ""),
                //new WordToPageMap("0631.jpg", "", ""),
                //new WordToPageMap("0632.jpg", "", ""),
                //new WordToPageMap("0633.jpg", "", ""),
                //new WordToPageMap("0634.jpg", "", ""),
                //new WordToPageMap("0635.jpg", "", ""),
                //new WordToPageMap("0636.jpg", "", ""),
                //new WordToPageMap("0637.jpg", "", ""),
                //new WordToPageMap("0638.jpg", "", ""),
                //new WordToPageMap("0639.jpg", "", ""),
                //new WordToPageMap("0640.jpg", "", ""),
                //new WordToPageMap("0641.jpg", "", ""),
                //new WordToPageMap("0642.jpg", "", ""),
                //new WordToPageMap("0643.jpg", "", ""),
                //new WordToPageMap("0644.jpg", "", ""),
                //new WordToPageMap("0645.jpg", "", ""),
                //new WordToPageMap("0646.jpg", "", ""),
                //new WordToPageMap("0647.jpg", "", ""),
                //new WordToPageMap("0648.jpg", "", ""),
                //new WordToPageMap("0649.jpg", "", ""),
                //new WordToPageMap("0650.jpg", "", ""),
                //new WordToPageMap("0651.jpg", "", ""),
                //new WordToPageMap("0652.jpg", "", ""),
                //new WordToPageMap("0653.jpg", "", ""),
                //new WordToPageMap("0654.jpg", "", ""),
                //new WordToPageMap("0655.jpg", "", ""),
                //new WordToPageMap("0656.jpg", "", ""),
                //new WordToPageMap("0657.jpg", "", ""),
                //new WordToPageMap("0658.jpg", "", ""),
                //new WordToPageMap("0659.jpg", "", ""),
                //new WordToPageMap("0660.jpg", "", ""),
                //new WordToPageMap("0661.jpg", "", ""),
                //new WordToPageMap("0662.jpg", "", ""),
                //new WordToPageMap("0663.jpg", "", ""),
                //new WordToPageMap("0664.jpg", "", ""),
                //new WordToPageMap("0665.jpg", "", ""),
                //new WordToPageMap("0666.jpg", "", ""),
                //new WordToPageMap("0667.jpg", "", ""),
                //new WordToPageMap("0668.jpg", "", ""),
                //new WordToPageMap("0669.jpg", "", ""),
                //new WordToPageMap("0670.jpg", "", ""),
                //new WordToPageMap("0671.jpg", "", ""),
                //new WordToPageMap("0672.jpg", "", ""),
                //new WordToPageMap("0673.jpg", "", ""),
                //new WordToPageMap("0674.jpg", "", ""),
                //new WordToPageMap("0675.jpg", "", ""),
                //new WordToPageMap("0676.jpg", "", ""),
                //new WordToPageMap("0677.jpg", "", ""),
                //new WordToPageMap("0678.jpg", "", ""),
                //new WordToPageMap("0679.jpg", "", ""),
                //new WordToPageMap("0680.jpg", "", ""),
                //new WordToPageMap("0681.jpg", "", ""),
                //new WordToPageMap("0682.jpg", "", ""),
                //new WordToPageMap("0683.jpg", "", ""),
                //new WordToPageMap("0684.jpg", "", ""),
                //new WordToPageMap("0685.jpg", "", ""),
                //new WordToPageMap("0686.jpg", "", ""),
                //new WordToPageMap("0687.jpg", "", ""),
                //new WordToPageMap("0688.jpg", "", ""),
                //new WordToPageMap("0689.jpg", "", ""),
                //new WordToPageMap("0690.jpg", "", ""),
                //new WordToPageMap("0691.jpg", "", ""),
                //new WordToPageMap("0692.jpg", "", ""),
                //new WordToPageMap("0693.jpg", "", ""),
                //new WordToPageMap("0694.jpg", "", ""),
                //new WordToPageMap("0695.jpg", "", ""),
                //new WordToPageMap("0696.jpg", "", ""),
                //new WordToPageMap("0697.jpg", "", ""),
                //new WordToPageMap("0698.jpg", "", ""),
                //new WordToPageMap("0699.jpg", "", ""),
                //new WordToPageMap("0700.jpg", "", ""),
                //new WordToPageMap("0701.jpg", "", ""),
                //new WordToPageMap("0702.jpg", "", ""),
                //new WordToPageMap("0703.jpg", "", ""),
                //new WordToPageMap("0704.jpg", "", ""),
                //new WordToPageMap("0705.jpg", "", ""),
                //new WordToPageMap("0706.jpg", "", ""),
                //new WordToPageMap("0707.jpg", "", ""),
                //new WordToPageMap("0708.jpg", "", ""),
                //new WordToPageMap("0709.jpg", "", ""),
                //new WordToPageMap("0710.jpg", "", ""),
                //new WordToPageMap("0711.jpg", "", ""),
                //new WordToPageMap("0712.jpg", "", ""),
                //new WordToPageMap("0713.jpg", "", ""),
                //new WordToPageMap("0714.jpg", "", ""),
                //new WordToPageMap("0715.jpg", "", ""),
                //new WordToPageMap("0716.jpg", "", ""),
                //new WordToPageMap("0717.jpg", "", ""),
                //new WordToPageMap("0718.jpg", "", ""),
                //new WordToPageMap("0719.jpg", "", ""),
                //new WordToPageMap("0720.jpg", "", ""),
                //new WordToPageMap("0721.jpg", "", ""),
                //new WordToPageMap("0722.jpg", "", ""),
                //new WordToPageMap("0723.jpg", "", ""),
                //new WordToPageMap("0724.jpg", "", ""),
                //new WordToPageMap("0725.jpg", "", ""),
                //new WordToPageMap("0726.jpg", "", ""),
                //new WordToPageMap("0727.jpg", "", ""),
                //new WordToPageMap("0728.jpg", "", ""),
                //new WordToPageMap("0729.jpg", "", ""),
                //new WordToPageMap("0730.jpg", "", ""),
                //new WordToPageMap("0731.jpg", "", ""),
                //new WordToPageMap("0732.jpg", "", ""),
                //new WordToPageMap("0733.jpg", "", ""),
                //new WordToPageMap("0734.jpg", "", ""),
                //new WordToPageMap("0735.jpg", "", ""),
                //new WordToPageMap("0736.jpg", "", ""),
                //new WordToPageMap("0737.jpg", "", ""),
                //new WordToPageMap("0738.jpg", "", ""),
                //new WordToPageMap("0739.jpg", "", ""),
                //new WordToPageMap("0740.jpg", "", ""),
                //new WordToPageMap("0741.jpg", "", ""),
                //new WordToPageMap("0742.jpg", "", ""),
                //new WordToPageMap("0743.jpg", "", ""),
                //new WordToPageMap("0744.jpg", "", ""),
                //new WordToPageMap("0745.jpg", "", ""),
                //new WordToPageMap("0746.jpg", "", ""),
                //new WordToPageMap("0747.jpg", "", ""),
                //new WordToPageMap("0748.jpg", "", ""),
                //new WordToPageMap("0749.jpg", "", ""),
                //new WordToPageMap("0750.jpg", "", ""),
                //new WordToPageMap("0751.jpg", "", ""),
                //new WordToPageMap("0752.jpg", "", ""),
                //new WordToPageMap("0753.jpg", "", ""),
                //new WordToPageMap("0754.jpg", "", ""),
                //new WordToPageMap("0755.jpg", "", ""),
                //new WordToPageMap("0756.jpg", "", ""),
                //new WordToPageMap("0757.jpg", "", ""),
                //new WordToPageMap("0758.jpg", "", ""),
                //new WordToPageMap("0759.jpg", "", ""),
                //new WordToPageMap("0760.jpg", "", ""),
                //new WordToPageMap("0761.jpg", "", ""),
                //new WordToPageMap("0762.jpg", "", ""),
                //new WordToPageMap("0763.jpg", "", ""),
                //new WordToPageMap("0764.jpg", "", ""),
                //new WordToPageMap("0765.jpg", "", ""),
                //new WordToPageMap("0766.jpg", "", ""),
                //new WordToPageMap("0767.jpg", "", ""),
                //new WordToPageMap("0768.jpg", "", ""),
                //new WordToPageMap("0769.jpg", "", ""),
                //new WordToPageMap("0770.jpg", "", ""),
                //new WordToPageMap("0771.jpg", "", ""),
                //new WordToPageMap("0772.jpg", "", ""),
                //new WordToPageMap("0773.jpg", "", ""),
                //new WordToPageMap("0774.jpg", "", ""),
                //new WordToPageMap("0775.jpg", "", ""),
                //new WordToPageMap("0776.jpg", "", ""),
                //new WordToPageMap("0777.jpg", "", ""),
                //new WordToPageMap("0778.jpg", "", ""),
                //new WordToPageMap("0779.jpg", "", ""),
                //new WordToPageMap("0780.jpg", "", ""),
                //new WordToPageMap("0781.jpg", "", ""),
                //new WordToPageMap("0782.jpg", "", ""),
                //new WordToPageMap("0783.jpg", "", ""),
                //new WordToPageMap("0784.jpg", "", ""),
                //new WordToPageMap("0785.jpg", "", ""),
                //new WordToPageMap("0786.jpg", "", ""),
                //new WordToPageMap("0787.jpg", "", ""),
                //new WordToPageMap("0788.jpg", "", ""),
                //new WordToPageMap("0789.jpg", "", ""),
                //new WordToPageMap("0790.jpg", "", ""),
                //new WordToPageMap("0791.jpg", "", ""),
                //new WordToPageMap("0792.jpg", "", ""),
                //new WordToPageMap("0793.jpg", "", ""),
                //new WordToPageMap("0794.jpg", "", ""),
                //new WordToPageMap("0795.jpg", "", ""),
                //new WordToPageMap("0796.jpg", "", ""),
                //new WordToPageMap("0797.jpg", "", ""),
            };
        }

        #endregion

        #region Fill the approx map

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = "reviewed")]
        private void FillTheApproxMap()
        {
            _wordsMapApproxDict.Add("0013.jpg", "abb");
            _wordsMapApproxDict.Add("0014.jpg", "abs");
            _wordsMapApproxDict.Add("0015.jpg", "adv");
            _wordsMapApproxDict.Add("0016.jpg", "adv");
            _wordsMapApproxDict.Add("0017.jpg", "aff");
            _wordsMapApproxDict.Add("0018.jpg", "aff");
            _wordsMapApproxDict.Add("0019.jpg", "afh");
            _wordsMapApproxDict.Add("0020.jpg", "afi");
            _wordsMapApproxDict.Add("0021.jpg", "afo");
            _wordsMapApproxDict.Add("0022.jpg", "afp");
            _wordsMapApproxDict.Add("0023.jpg", "afs");
            _wordsMapApproxDict.Add("0024.jpg", "afs");
            _wordsMapApproxDict.Add("0025.jpg", "aft");
            _wordsMapApproxDict.Add("0026.jpg", "aft");
            _wordsMapApproxDict.Add("0027.jpg", "agr");
            _wordsMapApproxDict.Add("0028.jpg", "agr");
            _wordsMapApproxDict.Add("0029.jpg", "alb");
            _wordsMapApproxDict.Add("0030.jpg", "alb");
            _wordsMapApproxDict.Add("0031.jpg", "alm");
            _wordsMapApproxDict.Add("0032.jpg", "alm");
            _wordsMapApproxDict.Add("0033.jpg", "amn");
            _wordsMapApproxDict.Add("0034.jpg", "amn");
            _wordsMapApproxDict.Add("0035.jpg", "ane");
            _wordsMapApproxDict.Add("0036.jpg", "ane");
            _wordsMapApproxDict.Add("0037.jpg", "ank");
            _wordsMapApproxDict.Add("0038.jpg", "ank");
            _wordsMapApproxDict.Add("0039.jpg", "ans");
            _wordsMapApproxDict.Add("0040.jpg", "ans");
            _wordsMapApproxDict.Add("0041.jpg", "ant");
            _wordsMapApproxDict.Add("0042.jpg", "ant");
            _wordsMapApproxDict.Add("0043.jpg", "apr");
            _wordsMapApproxDict.Add("0044.jpg", "apr");
            _wordsMapApproxDict.Add("0045.jpg", "ark");
            _wordsMapApproxDict.Add("0046.jpg", "ark");
            _wordsMapApproxDict.Add("0047.jpg", "arr");
            _wordsMapApproxDict.Add("0048.jpg", "asb");
            _wordsMapApproxDict.Add("0049.jpg", "att");
            _wordsMapApproxDict.Add("0050.jpg", "att");
            _wordsMapApproxDict.Add("0051.jpg", "bab");
            _wordsMapApproxDict.Add("0052.jpg", "bab");
            _wordsMapApproxDict.Add("0053.jpg", "bag");
            _wordsMapApproxDict.Add("0054.jpg", "bag");
            _wordsMapApproxDict.Add("0055.jpg", "bam");
            _wordsMapApproxDict.Add("0056.jpg", "ban");
            _wordsMapApproxDict.Add("0057.jpg", "bar");
            _wordsMapApproxDict.Add("0058.jpg", "bar");
            _wordsMapApproxDict.Add("0059.jpg", "beb");
            _wordsMapApproxDict.Add("0060.jpg", "beb");
            _wordsMapApproxDict.Add("0061.jpg", "bef");
            _wordsMapApproxDict.Add("0062.jpg", "bef");
            _wordsMapApproxDict.Add("0063.jpg", "beg");
            _wordsMapApproxDict.Add("0064.jpg", "beg");
            _wordsMapApproxDict.Add("0065.jpg", "bek");
            _wordsMapApproxDict.Add("0066.jpg", "bek");
            _wordsMapApproxDict.Add("0067.jpg", "ben");
            _wordsMapApproxDict.Add("0068.jpg", "ben");
            _wordsMapApproxDict.Add("0069.jpg", "ber");
            _wordsMapApproxDict.Add("0070.jpg", "ber");
            _wordsMapApproxDict.Add("0071.jpg", "bes");
            _wordsMapApproxDict.Add("0072.jpg", "bes");
            _wordsMapApproxDict.Add("0073.jpg", "bes");
            _wordsMapApproxDict.Add("0074.jpg", "bes");
            _wordsMapApproxDict.Add("0075.jpg", "bet");
            _wordsMapApproxDict.Add("0076.jpg", "bet");
            _wordsMapApproxDict.Add("0077.jpg", "bib");
            _wordsMapApproxDict.Add("0078.jpg", "bib");
            _wordsMapApproxDict.Add("0079.jpg", "bil");
            _wordsMapApproxDict.Add("0080.jpg", "bil");
            _wordsMapApproxDict.Add("0081.jpg", "bje");
            _wordsMapApproxDict.Add("0082.jpg", "bje");
            _wordsMapApproxDict.Add("0083.jpg", "bla");
            _wordsMapApproxDict.Add("0084.jpg", "bla");
            _wordsMapApproxDict.Add("0085.jpg", "blo");
            _wordsMapApproxDict.Add("0086.jpg", "blo");
            _wordsMapApproxDict.Add("0087.jpg", "blæ");
            _wordsMapApproxDict.Add("0088.jpg", "blæ");
            _wordsMapApproxDict.Add("0089.jpg", "bog");
            _wordsMapApproxDict.Add("0090.jpg", "bog");
            _wordsMapApproxDict.Add("0091.jpg", "bop");
            _wordsMapApproxDict.Add("0092.jpg", "bop");
            _wordsMapApproxDict.Add("0093.jpg", "bos");
            _wordsMapApproxDict.Add("0094.jpg", "bos");
            _wordsMapApproxDict.Add("0095.jpg", "bre");
            _wordsMapApproxDict.Add("0096.jpg", "bre");
            _wordsMapApproxDict.Add("0097.jpg", "bro");
            _wordsMapApproxDict.Add("0098.jpg", "bro");
            _wordsMapApproxDict.Add("0099.jpg", "bry");
            _wordsMapApproxDict.Add("0100.jpg", "bry");
            _wordsMapApproxDict.Add("0101.jpg", "brø");
            _wordsMapApproxDict.Add("0102.jpg", "brø");
            _wordsMapApproxDict.Add("0103.jpg", "bul");
            _wordsMapApproxDict.Add("0104.jpg", "bul");
            _wordsMapApproxDict.Add("0105.jpg", "byd");
            _wordsMapApproxDict.Add("0106.jpg", "byd");
            _wordsMapApproxDict.Add("0107.jpg", "bær");
            _wordsMapApproxDict.Add("0108.jpg", "bær");
            _wordsMapApproxDict.Add("0109.jpg", "bør");
            _wordsMapApproxDict.Add("0110.jpg", "bør");
            _wordsMapApproxDict.Add("0111.jpg", "cen");
            _wordsMapApproxDict.Add("0112.jpg", "cen");
            _wordsMapApproxDict.Add("0113.jpg", "cit");
            _wordsMapApproxDict.Add("0114.jpg", "cit");
            _wordsMapApproxDict.Add("0115.jpg", "dag");
            _wordsMapApproxDict.Add("0116.jpg", "dag");
            _wordsMapApproxDict.Add("0117.jpg", "dan");
            _wordsMapApproxDict.Add("0118.jpg", "dan");
            _wordsMapApproxDict.Add("0119.jpg", "dek");
            _wordsMapApproxDict.Add("0120.jpg", "dek");
            _wordsMapApproxDict.Add("0121.jpg", "dep");
            _wordsMapApproxDict.Add("0122.jpg", "dep");
            _wordsMapApproxDict.Add("0123.jpg", "des");
            _wordsMapApproxDict.Add("0124.jpg", "des");
            _wordsMapApproxDict.Add("0125.jpg", "dim");
            _wordsMapApproxDict.Add("0126.jpg", "dim");
            _wordsMapApproxDict.Add("0127.jpg", "dis");
            _wordsMapApproxDict.Add("0128.jpg", "dis");
            _wordsMapApproxDict.Add("0129.jpg", "dom");
            _wordsMapApproxDict.Add("0130.jpg", "dom");
            _wordsMapApproxDict.Add("0131.jpg", "dri");
            _wordsMapApproxDict.Add("0132.jpg", "dri");
            _wordsMapApproxDict.Add("0133.jpg", "drø");
            _wordsMapApproxDict.Add("0134.jpg", "drø");
            _wordsMapApproxDict.Add("0135.jpg", "duv");
            _wordsMapApproxDict.Add("0136.jpg", "duv");
            _wordsMapApproxDict.Add("0137.jpg", "dæk");
            _wordsMapApproxDict.Add("0138.jpg", "dæk");
            _wordsMapApproxDict.Add("0139.jpg", "døv");
            _wordsMapApproxDict.Add("0140.jpg", "døv");
            _wordsMapApproxDict.Add("0141.jpg", "eft");
            _wordsMapApproxDict.Add("0142.jpg", "eft");
            _wordsMapApproxDict.Add("0143.jpg", "egn");
            _wordsMapApproxDict.Add("0144.jpg", "egn");
            _wordsMapApproxDict.Add("0145.jpg", "eks");
            _wordsMapApproxDict.Add("0146.jpg", "eks");
            _wordsMapApproxDict.Add("0147.jpg", "eli");
            _wordsMapApproxDict.Add("0148.jpg", "eli");
            _wordsMapApproxDict.Add("0149.jpg", "end");
            _wordsMapApproxDict.Add("0150.jpg", "end");
            _wordsMapApproxDict.Add("0151.jpg", "enh");
            _wordsMapApproxDict.Add("0152.jpg", "eni");
            _wordsMapApproxDict.Add("0153.jpg", "erf");
            _wordsMapApproxDict.Add("0154.jpg", "erf");
            _wordsMapApproxDict.Add("0155.jpg", "etn");
            _wordsMapApproxDict.Add("0156.jpg", "etn");
            _wordsMapApproxDict.Add("0157.jpg", "fad");
            _wordsMapApproxDict.Add("0158.jpg", "fad");
            _wordsMapApproxDict.Add("0159.jpg", "fal");
            _wordsMapApproxDict.Add("0160.jpg", "fal");
            _wordsMapApproxDict.Add("0161.jpg", "far");
            _wordsMapApproxDict.Add("0162.jpg", "far");
            _wordsMapApproxDict.Add("0163.jpg", "fed");
            _wordsMapApproxDict.Add("0164.jpg", "fed");
            _wordsMapApproxDict.Add("0165.jpg", "fer");
            _wordsMapApproxDict.Add("0166.jpg", "fer");
            _wordsMapApproxDict.Add("0167.jpg", "fil");
            _wordsMapApproxDict.Add("0168.jpg", "fil");
            _wordsMapApproxDict.Add("0169.jpg", "fir");
            _wordsMapApproxDict.Add("0170.jpg", "fir");
            _wordsMapApproxDict.Add("0171.jpg", "fjo");
            _wordsMapApproxDict.Add("0172.jpg", "fjo");
            _wordsMapApproxDict.Add("0173.jpg", "fle");
            _wordsMapApproxDict.Add("0174.jpg", "fle");
            _wordsMapApproxDict.Add("0175.jpg", "fly");
            _wordsMapApproxDict.Add("0176.jpg", "fly");
            _wordsMapApproxDict.Add("0177.jpg", "fnu");
            _wordsMapApproxDict.Add("0178.jpg", "fnu");
            _wordsMapApproxDict.Add("0179.jpg", "fol");
            _wordsMapApproxDict.Add("0180.jpg", "fol");
            _wordsMapApproxDict.Add("0181.jpg", "for");
            _wordsMapApproxDict.Add("0182.jpg", "for");
            _wordsMapApproxDict.Add("0183.jpg", "for");
            _wordsMapApproxDict.Add("0184.jpg", "for");
            _wordsMapApproxDict.Add("0185.jpg", "for");
            _wordsMapApproxDict.Add("0186.jpg", "for");
            _wordsMapApproxDict.Add("0187.jpg", "for");
            _wordsMapApproxDict.Add("0188.jpg", "for");
            _wordsMapApproxDict.Add("0189.jpg", "for");
            _wordsMapApproxDict.Add("0190.jpg", "for");
            _wordsMapApproxDict.Add("0191.jpg", "for");
            _wordsMapApproxDict.Add("0192.jpg", "for");
            _wordsMapApproxDict.Add("0193.jpg", "for");
            _wordsMapApproxDict.Add("0194.jpg", "for");
            _wordsMapApproxDict.Add("0195.jpg", "for");
            _wordsMapApproxDict.Add("0196.jpg", "for");
            _wordsMapApproxDict.Add("0197.jpg", "for");
            _wordsMapApproxDict.Add("0198.jpg", "for");
            _wordsMapApproxDict.Add("0199.jpg", "for");
            _wordsMapApproxDict.Add("0200.jpg", "for");
            _wordsMapApproxDict.Add("0201.jpg", "for");
            _wordsMapApproxDict.Add("0202.jpg", "for");
            _wordsMapApproxDict.Add("0203.jpg", "for");
            _wordsMapApproxDict.Add("0204.jpg", "for");
            _wordsMapApproxDict.Add("0205.jpg", "foy");
            _wordsMapApproxDict.Add("0206.jpg", "fra");
            _wordsMapApproxDict.Add("0207.jpg", "fre");
            _wordsMapApproxDict.Add("0208.jpg", "fre");
            _wordsMapApproxDict.Add("0209.jpg", "fre");
            _wordsMapApproxDict.Add("0210.jpg", "fre");
            _wordsMapApproxDict.Add("0211.jpg", "fri");
            _wordsMapApproxDict.Add("0212.jpg", "fri");
            _wordsMapApproxDict.Add("0213.jpg", "fro");
            _wordsMapApproxDict.Add("0214.jpg", "fro");
            _wordsMapApproxDict.Add("0215.jpg", "ful");
            _wordsMapApproxDict.Add("0216.jpg", "ful");
            _wordsMapApproxDict.Add("0217.jpg", "fyl");
            _wordsMapApproxDict.Add("0218.jpg", "fyl");
            _wordsMapApproxDict.Add("0219.jpg", "fær");
            _wordsMapApproxDict.Add("0220.jpg", "fær");
            _wordsMapApproxDict.Add("0221.jpg", "før");
            _wordsMapApproxDict.Add("0222.jpg", "før");
            _wordsMapApproxDict.Add("0223.jpg", "gad");
            _wordsMapApproxDict.Add("0224.jpg", "gad");
            _wordsMapApproxDict.Add("0225.jpg", "gan");
            _wordsMapApproxDict.Add("0226.jpg", "gan");
            _wordsMapApproxDict.Add("0227.jpg", "geb");
            _wordsMapApproxDict.Add("0228.jpg", "geb");
            _wordsMapApproxDict.Add("0229.jpg", "gen");
            _wordsMapApproxDict.Add("0230.jpg", "gen");
            _wordsMapApproxDict.Add("0231.jpg", "gen");
            _wordsMapApproxDict.Add("0232.jpg", "gib");
            _wordsMapApproxDict.Add("0233.jpg", "gib");
            _wordsMapApproxDict.Add("0234.jpg", "gla");
            _wordsMapApproxDict.Add("0235.jpg", "gla");
            _wordsMapApproxDict.Add("0236.jpg", "gla");
            _wordsMapApproxDict.Add("0237.jpg", "gni");
            _wordsMapApproxDict.Add("0238.jpg", "gni");
            _wordsMapApproxDict.Add("0239.jpg", "gra");
            _wordsMapApproxDict.Add("0240.jpg", "gra");
            _wordsMapApproxDict.Add("0241.jpg", "gro");
            _wordsMapApproxDict.Add("0242.jpg", "gro");
            _wordsMapApproxDict.Add("0243.jpg", "gru");
            _wordsMapApproxDict.Add("0244.jpg", "gru");
            _wordsMapApproxDict.Add("0245.jpg", "grø");
            _wordsMapApproxDict.Add("0246.jpg", "grø");
            _wordsMapApproxDict.Add("0247.jpg", "gum");
            _wordsMapApproxDict.Add("0248.jpg", "gum");
            _wordsMapApproxDict.Add("0249.jpg", "gød");
            _wordsMapApproxDict.Add("0250.jpg", "gøe");
            _wordsMapApproxDict.Add("0251.jpg", "gåt");
            _wordsMapApproxDict.Add("0252.jpg", "hab");
            _wordsMapApproxDict.Add("0253.jpg", "hal");
            _wordsMapApproxDict.Add("0254.jpg", "hal");
            _wordsMapApproxDict.Add("0255.jpg", "han");
            _wordsMapApproxDict.Add("0256.jpg", "han");
            _wordsMapApproxDict.Add("0257.jpg", "hat");
            _wordsMapApproxDict.Add("0258.jpg", "hat");
            _wordsMapApproxDict.Add("0259.jpg", "heg");
            _wordsMapApproxDict.Add("0260.jpg", "hej");
            _wordsMapApproxDict.Add("0261.jpg", "hel");
            _wordsMapApproxDict.Add("0262.jpg", "hem");
            _wordsMapApproxDict.Add("0263.jpg", "hen");
            _wordsMapApproxDict.Add("0264.jpg", "hen");
            _wordsMapApproxDict.Add("0265.jpg", "hes");
            _wordsMapApproxDict.Add("0266.jpg", "hes");
            _wordsMapApproxDict.Add("0267.jpg", "hje");
            _wordsMapApproxDict.Add("0268.jpg", "hje");
            _wordsMapApproxDict.Add("0269.jpg", "hje");
            _wordsMapApproxDict.Add("0270.jpg", "hje");
            _wordsMapApproxDict.Add("0271.jpg", "hol");
            _wordsMapApproxDict.Add("0272.jpg", "hol");
            _wordsMapApproxDict.Add("0273.jpg", "hov");
            _wordsMapApproxDict.Add("0274.jpg", "hov");
            _wordsMapApproxDict.Add("0275.jpg", "hug");
            _wordsMapApproxDict.Add("0276.jpg", "hug");
            _wordsMapApproxDict.Add("0277.jpg", "hur");
            _wordsMapApproxDict.Add("0278.jpg", "hur");
            _wordsMapApproxDict.Add("0279.jpg", "hve");
            _wordsMapApproxDict.Add("0280.jpg", "hve");
            _wordsMapApproxDict.Add("0281.jpg", "hyd");
            _wordsMapApproxDict.Add("0282.jpg", "hyd");
            _wordsMapApproxDict.Add("0283.jpg", "hæl");
            _wordsMapApproxDict.Add("0284.jpg", "hæl");
            _wordsMapApproxDict.Add("0285.jpg", "høj");
            _wordsMapApproxDict.Add("0286.jpg", "høj");
            _wordsMapApproxDict.Add("0287.jpg", "hør");
            _wordsMapApproxDict.Add("0288.jpg", "hør");
            _wordsMapApproxDict.Add("0289.jpg", "hår");
            _wordsMapApproxDict.Add("0290.jpg", "hår");
            _wordsMapApproxDict.Add("0291.jpg", "ifa");
            _wordsMapApproxDict.Add("0292.jpg", "ifa");
            _wordsMapApproxDict.Add("0293.jpg", "ill");
            _wordsMapApproxDict.Add("0294.jpg", "ill");
            _wordsMapApproxDict.Add("0295.jpg", "ind");
            _wordsMapApproxDict.Add("0296.jpg", "ind");
            _wordsMapApproxDict.Add("0297.jpg", "ind");
            _wordsMapApproxDict.Add("0298.jpg", "ind");
            _wordsMapApproxDict.Add("0299.jpg", "ind");
            _wordsMapApproxDict.Add("0300.jpg", "ind");
            _wordsMapApproxDict.Add("0301.jpg", "ind");
            _wordsMapApproxDict.Add("0302.jpg", "ind");
            _wordsMapApproxDict.Add("0303.jpg", "ind");
            _wordsMapApproxDict.Add("0304.jpg", "ind");
            _wordsMapApproxDict.Add("0305.jpg", "inf");
            _wordsMapApproxDict.Add("0306.jpg", "inf");
            _wordsMapApproxDict.Add("0307.jpg", "ins");
            _wordsMapApproxDict.Add("0308.jpg", "ins");
            _wordsMapApproxDict.Add("0309.jpg", "inv");
            _wordsMapApproxDict.Add("0310.jpg", "inv");
            _wordsMapApproxDict.Add("0311.jpg", "ivæ");
            _wordsMapApproxDict.Add("0312.jpg", "ivæ");
            _wordsMapApproxDict.Add("0313.jpg", "jer");
            _wordsMapApproxDict.Add("0314.jpg", "jer");
            _wordsMapApproxDict.Add("0315.jpg", "jul");
            _wordsMapApproxDict.Add("0316.jpg", "jul");
            _wordsMapApproxDict.Add("0317.jpg", "kad");
            _wordsMapApproxDict.Add("0318.jpg", "kag");
            _wordsMapApproxDict.Add("0319.jpg", "kam");
            _wordsMapApproxDict.Add("0320.jpg", "kam");
            _wordsMapApproxDict.Add("0321.jpg", "kap");
            _wordsMapApproxDict.Add("0322.jpg", "kap");
            _wordsMapApproxDict.Add("0323.jpg", "kar");
            _wordsMapApproxDict.Add("0324.jpg", "kar");
            _wordsMapApproxDict.Add("0325.jpg", "kat");
            _wordsMapApproxDict.Add("0326.jpg", "kat");
            _wordsMapApproxDict.Add("0327.jpg", "ker");
            _wordsMapApproxDict.Add("0328.jpg", "ker");
            _wordsMapApproxDict.Add("0329.jpg", "kjo");
            _wordsMapApproxDict.Add("0330.jpg", "kjo");
            _wordsMapApproxDict.Add("0331.jpg", "kle");
            _wordsMapApproxDict.Add("0332.jpg", "kli");
            _wordsMapApproxDict.Add("0333.jpg", "klu");
            _wordsMapApproxDict.Add("0334.jpg", "klu");
            _wordsMapApproxDict.Add("0335.jpg", "kne");
            _wordsMapApproxDict.Add("0336.jpg", "kne");
            _wordsMapApproxDict.Add("0337.jpg", "kno");
            _wordsMapApproxDict.Add("0338.jpg", "koa");
            _wordsMapApproxDict.Add("0339.jpg", "kol");
            _wordsMapApproxDict.Add("0340.jpg", "kol");
            _wordsMapApproxDict.Add("0341.jpg", "kom");
            _wordsMapApproxDict.Add("0342.jpg", "kom");
            _wordsMapApproxDict.Add("0343.jpg", "kon");
            _wordsMapApproxDict.Add("0344.jpg", "kon");
            _wordsMapApproxDict.Add("0345.jpg", "kon");
            _wordsMapApproxDict.Add("0346.jpg", "kon");
            _wordsMapApproxDict.Add("0347.jpg", "kon");
            _wordsMapApproxDict.Add("0348.jpg", "kon");
            _wordsMapApproxDict.Add("0349.jpg", "kor");
            _wordsMapApproxDict.Add("0350.jpg", "kor");
            _wordsMapApproxDict.Add("0351.jpg", "kra");
            _wordsMapApproxDict.Add("0352.jpg", "kra");
            _wordsMapApproxDict.Add("0353.jpg", "kri");
            _wordsMapApproxDict.Add("0354.jpg", "kri");
            _wordsMapApproxDict.Add("0355.jpg", "kro");
            _wordsMapApproxDict.Add("0356.jpg", "kro");
            _wordsMapApproxDict.Add("0357.jpg", "kry");
            _wordsMapApproxDict.Add("0358.jpg", "kry");
            _wordsMapApproxDict.Add("0359.jpg", "kul");
            _wordsMapApproxDict.Add("0360.jpg", "kul");
            _wordsMapApproxDict.Add("0361.jpg", "kup");
            _wordsMapApproxDict.Add("0362.jpg", "kup");
            _wordsMapApproxDict.Add("0363.jpg", "kvi");
            _wordsMapApproxDict.Add("0364.jpg", "kvi");
            _wordsMapApproxDict.Add("0365.jpg", "kys");
            _wordsMapApproxDict.Add("0366.jpg", "kys");
            _wordsMapApproxDict.Add("0367.jpg", "køb");
            _wordsMapApproxDict.Add("0368.jpg", "køb");
            _wordsMapApproxDict.Add("0369.jpg", "lab");
            _wordsMapApproxDict.Add("0370.jpg", "lab");
            _wordsMapApproxDict.Add("0371.jpg", "lam");
            _wordsMapApproxDict.Add("0372.jpg", "lam");
            _wordsMapApproxDict.Add("0373.jpg", "lan");
            _wordsMapApproxDict.Add("0374.jpg", "lan");
            _wordsMapApproxDict.Add("0375.jpg", "las");
            _wordsMapApproxDict.Add("0376.jpg", "las");
            _wordsMapApproxDict.Add("0377.jpg", "led");
            _wordsMapApproxDict.Add("0378.jpg", "led");
            _wordsMapApproxDict.Add("0379.jpg", "lem");
            _wordsMapApproxDict.Add("0380.jpg", "lem");
            _wordsMapApproxDict.Add("0381.jpg", "lev");
            _wordsMapApproxDict.Add("0382.jpg", "lev");
            _wordsMapApproxDict.Add("0383.jpg", "lig");
            _wordsMapApproxDict.Add("0384.jpg", "lig");
            _wordsMapApproxDict.Add("0385.jpg", "lin");
            _wordsMapApproxDict.Add("0386.jpg", "lin");
            _wordsMapApproxDict.Add("0387.jpg", "liv");
            _wordsMapApproxDict.Add("0388.jpg", "liv");
            _wordsMapApproxDict.Add("0389.jpg", "lom");
            _wordsMapApproxDict.Add("0390.jpg", "lom");
            _wordsMapApproxDict.Add("0391.jpg", "luf");
            _wordsMapApproxDict.Add("0392.jpg", "luf");
            _wordsMapApproxDict.Add("0393.jpg", "lul");
            _wordsMapApproxDict.Add("0394.jpg", "lum");
            _wordsMapApproxDict.Add("0395.jpg", "lyd");
            _wordsMapApproxDict.Add("0396.jpg", "lyd");
            _wordsMapApproxDict.Add("0397.jpg", "lys");
            _wordsMapApproxDict.Add("0398.jpg", "lys");
            _wordsMapApproxDict.Add("0399.jpg", "læg");
            _wordsMapApproxDict.Add("0400.jpg", "læg");
            _wordsMapApproxDict.Add("0401.jpg", "lær");
            _wordsMapApproxDict.Add("0402.jpg", "lær");
            _wordsMapApproxDict.Add("0403.jpg", "løf");
            _wordsMapApproxDict.Add("0404.jpg", "løf");
            _wordsMapApproxDict.Add("0405.jpg", "løs");
            _wordsMapApproxDict.Add("0406.jpg", "løs");
            _wordsMapApproxDict.Add("0407.jpg", "mad");
            _wordsMapApproxDict.Add("0408.jpg", "mad");
            _wordsMapApproxDict.Add("0409.jpg", "maj");
            _wordsMapApproxDict.Add("0410.jpg", "maj");
            _wordsMapApproxDict.Add("0411.jpg", "man");
            _wordsMapApproxDict.Add("0412.jpg", "man");
            _wordsMapApproxDict.Add("0413.jpg", "mar");
            _wordsMapApproxDict.Add("0414.jpg", "mar");
            _wordsMapApproxDict.Add("0415.jpg", "mas");
            _wordsMapApproxDict.Add("0416.jpg", "mas");
            _wordsMapApproxDict.Add("0417.jpg", "mat");
            _wordsMapApproxDict.Add("0418.jpg", "mat");
            _wordsMapApproxDict.Add("0419.jpg", "med");
            _wordsMapApproxDict.Add("0420.jpg", "med");
            _wordsMapApproxDict.Add("0421.jpg", "mel");
            _wordsMapApproxDict.Add("0422.jpg", "mel");
            _wordsMapApproxDict.Add("0423.jpg", "men");
            _wordsMapApproxDict.Add("0424.jpg", "men");
            _wordsMapApproxDict.Add("0425.jpg", "met");
            _wordsMapApproxDict.Add("0426.jpg", "met");
            _wordsMapApproxDict.Add("0427.jpg", "mil");
            _wordsMapApproxDict.Add("0428.jpg", "mil");
            _wordsMapApproxDict.Add("0429.jpg", "min");
            _wordsMapApproxDict.Add("0430.jpg", "min");
            _wordsMapApproxDict.Add("0431.jpg", "mod");
            _wordsMapApproxDict.Add("0432.jpg", "mod");
            _wordsMapApproxDict.Add("0433.jpg", "mod");
            _wordsMapApproxDict.Add("0434.jpg", "mod");
            _wordsMapApproxDict.Add("0435.jpg", "mor");
            _wordsMapApproxDict.Add("0436.jpg", "mor");
            _wordsMapApproxDict.Add("0437.jpg", "mot");
            _wordsMapApproxDict.Add("0438.jpg", "mot");
            _wordsMapApproxDict.Add("0439.jpg", "mur");
            _wordsMapApproxDict.Add("0440.jpg", "mur");
            _wordsMapApproxDict.Add("0441.jpg", "mæl");
            _wordsMapApproxDict.Add("0442.jpg", "mæl");
            _wordsMapApproxDict.Add("0443.jpg", "møn");
            _wordsMapApproxDict.Add("0444.jpg", "møn");
            _wordsMapApproxDict.Add("0445.jpg", "mån");
            _wordsMapApproxDict.Add("0446.jpg", "mån");
            _wordsMapApproxDict.Add("0447.jpg", "nat");
            _wordsMapApproxDict.Add("0448.jpg", "nat");
            _wordsMapApproxDict.Add("0449.jpg", "nav");
            _wordsMapApproxDict.Add("0450.jpg", "nav");
            _wordsMapApproxDict.Add("0451.jpg", "ned");
            _wordsMapApproxDict.Add("0452.jpg", "ned");
            _wordsMapApproxDict.Add("0453.jpg", "ner");
            _wordsMapApproxDict.Add("0454.jpg", "ner");
            _wordsMapApproxDict.Add("0455.jpg", "nog");
            _wordsMapApproxDict.Add("0456.jpg", "nog");
            _wordsMapApproxDict.Add("0457.jpg", "not");
            _wordsMapApproxDict.Add("0458.jpg", "not");
            _wordsMapApproxDict.Add("0459.jpg", "nyr");
            _wordsMapApproxDict.Add("0460.jpg", "nys");
            _wordsMapApproxDict.Add("0461.jpg", "næs");
            _wordsMapApproxDict.Add("0462.jpg", "næs");
            _wordsMapApproxDict.Add("0463.jpg", "nød");
            _wordsMapApproxDict.Add("0464.jpg", "nåd");
            _wordsMapApproxDict.Add("0465.jpg", "off");
            _wordsMapApproxDict.Add("0466.jpg", "off");
            _wordsMapApproxDict.Add("0467.jpg", "omb");
            _wordsMapApproxDict.Add("0468.jpg", "omb");
            _wordsMapApproxDict.Add("0469.jpg", "omk");
            _wordsMapApproxDict.Add("0470.jpg", "omk");
            _wordsMapApproxDict.Add("0471.jpg", "oms");
            _wordsMapApproxDict.Add("0472.jpg", "oms");
            _wordsMapApproxDict.Add("0473.jpg", "opb");
            _wordsMapApproxDict.Add("0474.jpg", "opb");
            _wordsMapApproxDict.Add("0475.jpg", "opf");
            _wordsMapApproxDict.Add("0476.jpg", "opf");
            _wordsMapApproxDict.Add("0477.jpg", "opl");
            _wordsMapApproxDict.Add("0478.jpg", "opl");
            _wordsMapApproxDict.Add("0479.jpg", "opp");
            _wordsMapApproxDict.Add("0480.jpg", "opp");
            _wordsMapApproxDict.Add("0481.jpg", "ops");
            _wordsMapApproxDict.Add("0482.jpg", "ops");
            _wordsMapApproxDict.Add("0483.jpg", "opt");
            _wordsMapApproxDict.Add("0484.jpg", "opt");
            _wordsMapApproxDict.Add("0485.jpg", "ord");
            _wordsMapApproxDict.Add("0486.jpg", "ord");
            _wordsMapApproxDict.Add("0487.jpg", "ost");
            _wordsMapApproxDict.Add("0488.jpg", "ost");
            _wordsMapApproxDict.Add("0489.jpg", "ove");
            _wordsMapApproxDict.Add("0490.jpg", "ove");
            _wordsMapApproxDict.Add("0491.jpg", "ove");
            _wordsMapApproxDict.Add("0492.jpg", "ove");
            _wordsMapApproxDict.Add("0493.jpg", "ove");
            _wordsMapApproxDict.Add("0494.jpg", "ove");
            _wordsMapApproxDict.Add("0495.jpg", "ove");
            _wordsMapApproxDict.Add("0496.jpg", "ove");
            _wordsMapApproxDict.Add("0497.jpg", "pal");
            _wordsMapApproxDict.Add("0498.jpg", "pal");
            _wordsMapApproxDict.Add("0499.jpg", "par");
            _wordsMapApproxDict.Add("0500.jpg", "par");
            _wordsMapApproxDict.Add("0501.jpg", "par");
            _wordsMapApproxDict.Add("0502.jpg", "par");
            _wordsMapApproxDict.Add("0503.jpg", "pat");
            _wordsMapApproxDict.Add("0504.jpg", "pat");
            _wordsMapApproxDict.Add("0505.jpg", "pen");
            _wordsMapApproxDict.Add("0506.jpg", "pen");
            _wordsMapApproxDict.Add("0507.jpg", "per");
            _wordsMapApproxDict.Add("0508.jpg", "per");
            _wordsMapApproxDict.Add("0509.jpg", "pin");
            _wordsMapApproxDict.Add("0510.jpg", "pin");
            _wordsMapApproxDict.Add("0511.jpg", "pla");
            _wordsMapApproxDict.Add("0512.jpg", "pla");
            _wordsMapApproxDict.Add("0513.jpg", "plo");
            _wordsMapApproxDict.Add("0514.jpg", "plo");
            _wordsMapApproxDict.Add("0515.jpg", "pol");
            _wordsMapApproxDict.Add("0516.jpg", "pol");
            _wordsMapApproxDict.Add("0517.jpg", "pos");
            _wordsMapApproxDict.Add("0518.jpg", "pos");
            _wordsMapApproxDict.Add("0519.jpg", "pre");
            _wordsMapApproxDict.Add("0520.jpg", "pre");
            _wordsMapApproxDict.Add("0521.jpg", "pro");
            _wordsMapApproxDict.Add("0522.jpg", "pro");
            _wordsMapApproxDict.Add("0523.jpg", "pro");
            _wordsMapApproxDict.Add("0524.jpg", "pro");
            _wordsMapApproxDict.Add("0525.jpg", "præ");
            _wordsMapApproxDict.Add("0526.jpg", "præ");
            _wordsMapApproxDict.Add("0527.jpg", "pum");
            _wordsMapApproxDict.Add("0528.jpg", "pum");
            _wordsMapApproxDict.Add("0529.jpg", "pyg");
            _wordsMapApproxDict.Add("0530.jpg", "påa");
            _wordsMapApproxDict.Add("0531.jpg", "pås");
            _wordsMapApproxDict.Add("0532.jpg", "pås");
            _wordsMapApproxDict.Add("0533.jpg", "rad");
            _wordsMapApproxDict.Add("0534.jpg", "rad");
            _wordsMapApproxDict.Add("0535.jpg", "ran");
            _wordsMapApproxDict.Add("0536.jpg", "ran");
            _wordsMapApproxDict.Add("0537.jpg", "rea");
            _wordsMapApproxDict.Add("0538.jpg", "rea");
            _wordsMapApproxDict.Add("0539.jpg", "ref");
            _wordsMapApproxDict.Add("0540.jpg", "ref");
            _wordsMapApproxDict.Add("0541.jpg", "reg");
            _wordsMapApproxDict.Add("0542.jpg", "reg");
            _wordsMapApproxDict.Add("0543.jpg", "rek");
            _wordsMapApproxDict.Add("0544.jpg", "rek");
            _wordsMapApproxDict.Add("0545.jpg", "ren");
            _wordsMapApproxDict.Add("0546.jpg", "ren");
            _wordsMapApproxDict.Add("0547.jpg", "res");
            _wordsMapApproxDict.Add("0548.jpg", "res");
            _wordsMapApproxDict.Add("0549.jpg", "ret");
            _wordsMapApproxDict.Add("0550.jpg", "ret");
            _wordsMapApproxDict.Add("0551.jpg", "rib");
            _wordsMapApproxDict.Add("0552.jpg", "rib");
            _wordsMapApproxDict.Add("0553.jpg", "rin");
            _wordsMapApproxDict.Add("0554.jpg", "rin");
            _wordsMapApproxDict.Add("0555.jpg", "rod");
            _wordsMapApproxDict.Add("0556.jpg", "rod");
            _wordsMapApproxDict.Add("0557.jpg", "roy");
            _wordsMapApproxDict.Add("0558.jpg", "rub");
            _wordsMapApproxDict.Add("0559.jpg", "run");
            _wordsMapApproxDict.Add("0560.jpg", "run");
            _wordsMapApproxDict.Add("0561.jpg", "ryg");
            _wordsMapApproxDict.Add("0562.jpg", "ryg");
            _wordsMapApproxDict.Add("0563.jpg", "rød");
            _wordsMapApproxDict.Add("0564.jpg", "rød");
            _wordsMapApproxDict.Add("0565.jpg", "rør");
            _wordsMapApproxDict.Add("0566.jpg", "rør");
            _wordsMapApproxDict.Add("0567.jpg", "sad");
            _wordsMapApproxDict.Add("0568.jpg", "sad");
            _wordsMapApproxDict.Add("0569.jpg", "sal");
            _wordsMapApproxDict.Add("0570.jpg", "sal");
            _wordsMapApproxDict.Add("0571.jpg", "sam");
            _wordsMapApproxDict.Add("0572.jpg", "sam");
            _wordsMapApproxDict.Add("0573.jpg", "sam");
            _wordsMapApproxDict.Add("0574.jpg", "sam");
            _wordsMapApproxDict.Add("0575.jpg", "san");
            _wordsMapApproxDict.Add("0576.jpg", "san");
            _wordsMapApproxDict.Add("0577.jpg", "sat");
            _wordsMapApproxDict.Add("0578.jpg", "sat");
            _wordsMapApproxDict.Add("0579.jpg", "sej");
            _wordsMapApproxDict.Add("0580.jpg", "sej");
            _wordsMapApproxDict.Add("0581.jpg", "sel");
            _wordsMapApproxDict.Add("0582.jpg", "sel");
            _wordsMapApproxDict.Add("0583.jpg", "sen");
            _wordsMapApproxDict.Add("0584.jpg", "sen");
            _wordsMapApproxDict.Add("0585.jpg", "sid");
            _wordsMapApproxDict.Add("0586.jpg", "sid");
            _wordsMapApproxDict.Add("0587.jpg", "sig");
            _wordsMapApproxDict.Add("0588.jpg", "sig");
            _wordsMapApproxDict.Add("0589.jpg", "sin");
            _wordsMapApproxDict.Add("0590.jpg", "sin");
            _wordsMapApproxDict.Add("0591.jpg", "ska");
            _wordsMapApproxDict.Add("0592.jpg", "ska");
            _wordsMapApproxDict.Add("0593.jpg", "ska");
            _wordsMapApproxDict.Add("0594.jpg", "ska");
            _wordsMapApproxDict.Add("0595.jpg", "ski");
            _wordsMapApproxDict.Add("0596.jpg", "ski");
            _wordsMapApproxDict.Add("0597.jpg", "ski");
            _wordsMapApproxDict.Add("0598.jpg", "ski");
            _wordsMapApproxDict.Add("0599.jpg", "sko");
            _wordsMapApproxDict.Add("0600.jpg", "sko");
            _wordsMapApproxDict.Add("0601.jpg", "skr");
            _wordsMapApproxDict.Add("0602.jpg", "skr");
            _wordsMapApproxDict.Add("0603.jpg", "skr");
            _wordsMapApproxDict.Add("0604.jpg", "skr");
            _wordsMapApproxDict.Add("0605.jpg", "sku");
            _wordsMapApproxDict.Add("0606.jpg", "sku");
            _wordsMapApproxDict.Add("0607.jpg", "sky");
            _wordsMapApproxDict.Add("0608.jpg", "sky");
            _wordsMapApproxDict.Add("0609.jpg", "skæ");
            _wordsMapApproxDict.Add("0610.jpg", "skæ");
            _wordsMapApproxDict.Add("0611.jpg", "skø");
            _wordsMapApproxDict.Add("0612.jpg", "skø");
            _wordsMapApproxDict.Add("0613.jpg", "sla");
            _wordsMapApproxDict.Add("0614.jpg", "sla");
            _wordsMapApproxDict.Add("0615.jpg", "sli");
            _wordsMapApproxDict.Add("0616.jpg", "sli");
            _wordsMapApproxDict.Add("0617.jpg", "slæ");
            _wordsMapApproxDict.Add("0618.jpg", "slæ");
            _wordsMapApproxDict.Add("0619.jpg", "sma");
            _wordsMapApproxDict.Add("0620.jpg", "sma");
            _wordsMapApproxDict.Add("0621.jpg", "smu");
            _wordsMapApproxDict.Add("0622.jpg", "smu");
            _wordsMapApproxDict.Add("0623.jpg", "små");
            _wordsMapApproxDict.Add("0624.jpg", "små");
            _wordsMapApproxDict.Add("0625.jpg", "sne");
            _wordsMapApproxDict.Add("0626.jpg", "sne");
            _wordsMapApproxDict.Add("0627.jpg", "snæ");
            _wordsMapApproxDict.Add("0628.jpg", "snæ");
            _wordsMapApproxDict.Add("0629.jpg", "sol");
            _wordsMapApproxDict.Add("0630.jpg", "sol");
            _wordsMapApproxDict.Add("0631.jpg", "sov");
            _wordsMapApproxDict.Add("0632.jpg", "sov");
            _wordsMapApproxDict.Add("0633.jpg", "spe");
            _wordsMapApproxDict.Add("0634.jpg", "spe");
            _wordsMapApproxDict.Add("0635.jpg", "spi");
            _wordsMapApproxDict.Add("0636.jpg", "spi");
            _wordsMapApproxDict.Add("0637.jpg", "spo");
            _wordsMapApproxDict.Add("0638.jpg", "spo");
            _wordsMapApproxDict.Add("0639.jpg", "spr");
            _wordsMapApproxDict.Add("0640.jpg", "spr");
            _wordsMapApproxDict.Add("0641.jpg", "spæ");
            _wordsMapApproxDict.Add("0642.jpg", "spæ");
            _wordsMapApproxDict.Add("0643.jpg", "sta");
            _wordsMapApproxDict.Add("0644.jpg", "sta");
            _wordsMapApproxDict.Add("0645.jpg", "sta");
            _wordsMapApproxDict.Add("0646.jpg", "sta");
            _wordsMapApproxDict.Add("0647.jpg", "ste");
            _wordsMapApproxDict.Add("0648.jpg", "ste");
            _wordsMapApproxDict.Add("0649.jpg", "ste");
            _wordsMapApproxDict.Add("0650.jpg", "ste");
            _wordsMapApproxDict.Add("0651.jpg", "sti");
            _wordsMapApproxDict.Add("0652.jpg", "sti");
            _wordsMapApproxDict.Add("0653.jpg", "sto");
            _wordsMapApproxDict.Add("0654.jpg", "sto");
            _wordsMapApproxDict.Add("0655.jpg", "sto");
            _wordsMapApproxDict.Add("0656.jpg", "sto");
            _wordsMapApproxDict.Add("0657.jpg", "str");
            _wordsMapApproxDict.Add("0658.jpg", "str");
            _wordsMapApproxDict.Add("0659.jpg", "str");
            _wordsMapApproxDict.Add("0660.jpg", "str");
            _wordsMapApproxDict.Add("0661.jpg", "stu");
            _wordsMapApproxDict.Add("0662.jpg", "stu");
            _wordsMapApproxDict.Add("0663.jpg", "sty");
            _wordsMapApproxDict.Add("0664.jpg", "sty");
            _wordsMapApproxDict.Add("0665.jpg", "stø");
            _wordsMapApproxDict.Add("0666.jpg", "stø");
            _wordsMapApproxDict.Add("0667.jpg", "sub");
            _wordsMapApproxDict.Add("0668.jpg", "sub");
            _wordsMapApproxDict.Add("0669.jpg", "sun");
            _wordsMapApproxDict.Add("0670.jpg", "sun");
            _wordsMapApproxDict.Add("0671.jpg", "sut");
            _wordsMapApproxDict.Add("0672.jpg", "sva");
            _wordsMapApproxDict.Add("0673.jpg", "svi");
            _wordsMapApproxDict.Add("0674.jpg", "svi");
            _wordsMapApproxDict.Add("0675.jpg", "svæ");
            _wordsMapApproxDict.Add("0676.jpg", "svæ");
            _wordsMapApproxDict.Add("0677.jpg", "syk");
            _wordsMapApproxDict.Add("0678.jpg", "syl");
            _wordsMapApproxDict.Add("0679.jpg", "sys");
            _wordsMapApproxDict.Add("0680.jpg", "sys");
            _wordsMapApproxDict.Add("0681.jpg", "sæs");
            _wordsMapApproxDict.Add("0682.jpg", "sæt");
            _wordsMapApproxDict.Add("0683.jpg", "søl");
            _wordsMapApproxDict.Add("0684.jpg", "søl");
            _wordsMapApproxDict.Add("0685.jpg", "søs");
            _wordsMapApproxDict.Add("0686.jpg", "søs");
            _wordsMapApproxDict.Add("0687.jpg", "tag");
            _wordsMapApproxDict.Add("0688.jpg", "tag");
            _wordsMapApproxDict.Add("0689.jpg", "tal");
            _wordsMapApproxDict.Add("0690.jpg", "tal");
            _wordsMapApproxDict.Add("0691.jpg", "tap");
            _wordsMapApproxDict.Add("0692.jpg", "tap");
            _wordsMapApproxDict.Add("0693.jpg", "teg");
            _wordsMapApproxDict.Add("0694.jpg", "teg");
            _wordsMapApproxDict.Add("0695.jpg", "ter");
            _wordsMapApproxDict.Add("0696.jpg", "ter");
            _wordsMapApproxDict.Add("0697.jpg", "tid");
            _wordsMapApproxDict.Add("0698.jpg", "tid");
            _wordsMapApproxDict.Add("0699.jpg", "til");
            _wordsMapApproxDict.Add("0700.jpg", "til");
            _wordsMapApproxDict.Add("0701.jpg", "til");
            _wordsMapApproxDict.Add("0702.jpg", "til");
            _wordsMapApproxDict.Add("0703.jpg", "til");
            _wordsMapApproxDict.Add("0704.jpg", "til");
            _wordsMapApproxDict.Add("0705.jpg", "til");
            _wordsMapApproxDict.Add("0706.jpg", "til");
            _wordsMapApproxDict.Add("0707.jpg", "tje");
            _wordsMapApproxDict.Add("0708.jpg", "tje");
            _wordsMapApproxDict.Add("0709.jpg", "tom");
            _wordsMapApproxDict.Add("0710.jpg", "tom");
            _wordsMapApproxDict.Add("0711.jpg", "tot");
            _wordsMapApproxDict.Add("0712.jpg", "tot");
            _wordsMapApproxDict.Add("0713.jpg", "tra");
            _wordsMapApproxDict.Add("0714.jpg", "tra");
            _wordsMapApproxDict.Add("0715.jpg", "tri");
            _wordsMapApproxDict.Add("0716.jpg", "tri");
            _wordsMapApproxDict.Add("0717.jpg", "tro");
            _wordsMapApproxDict.Add("0718.jpg", "tro");
            _wordsMapApproxDict.Add("0719.jpg", "træ");
            _wordsMapApproxDict.Add("0720.jpg", "træ");
            _wordsMapApproxDict.Add("0721.jpg", "tsa");
            _wordsMapApproxDict.Add("0722.jpg", "tsa");
            _wordsMapApproxDict.Add("0723.jpg", "tut");
            _wordsMapApproxDict.Add("0724.jpg", "tut");
            _wordsMapApproxDict.Add("0725.jpg", "tyn");
            _wordsMapApproxDict.Add("0726.jpg", "tyn");
            _wordsMapApproxDict.Add("0727.jpg", "tæt");
            _wordsMapApproxDict.Add("0728.jpg", "tæt");
            _wordsMapApproxDict.Add("0729.jpg", "tøn");
            _wordsMapApproxDict.Add("0730.jpg", "tåb");
            _wordsMapApproxDict.Add("0731.jpg", "uar");
            _wordsMapApproxDict.Add("0732.jpg", "uar");
            _wordsMapApproxDict.Add("0733.jpg", "ubl");
            _wordsMapApproxDict.Add("0734.jpg", "ubo");
            _wordsMapApproxDict.Add("0735.jpg", "ude");
            _wordsMapApproxDict.Add("0736.jpg", "ude");
            _wordsMapApproxDict.Add("0737.jpg", "udg");
            _wordsMapApproxDict.Add("0738.jpg", "udg");
            _wordsMapApproxDict.Add("0739.jpg", "udm");
            _wordsMapApproxDict.Add("0740.jpg", "udm");
            _wordsMapApproxDict.Add("0741.jpg", "uds");
            _wordsMapApproxDict.Add("0742.jpg", "uds");
            _wordsMapApproxDict.Add("0743.jpg", "uds");
            _wordsMapApproxDict.Add("0744.jpg", "uds");
            _wordsMapApproxDict.Add("0745.jpg", "udy");
            _wordsMapApproxDict.Add("0746.jpg", "udy");
            _wordsMapApproxDict.Add("0747.jpg", "ufo");
            _wordsMapApproxDict.Add("0748.jpg", "ufo");
            _wordsMapApproxDict.Add("0749.jpg", "uha");
            _wordsMapApproxDict.Add("0750.jpg", "uig");
            _wordsMapApproxDict.Add("0751.jpg", "ulv");
            _wordsMapApproxDict.Add("0752.jpg", "ulv");
            _wordsMapApproxDict.Add("0753.jpg", "und");
            _wordsMapApproxDict.Add("0754.jpg", "und");
            _wordsMapApproxDict.Add("0755.jpg", "und");
            _wordsMapApproxDict.Add("0756.jpg", "und");
            _wordsMapApproxDict.Add("0757.jpg", "ung");
            _wordsMapApproxDict.Add("0758.jpg", "ung");
            _wordsMapApproxDict.Add("0759.jpg", "upa");
            _wordsMapApproxDict.Add("0760.jpg", "upa");
            _wordsMapApproxDict.Add("0761.jpg", "urt");
            _wordsMapApproxDict.Add("0762.jpg", "urt");
            _wordsMapApproxDict.Add("0763.jpg", "usæ");
            _wordsMapApproxDict.Add("0764.jpg", "usæ");
            _wordsMapApproxDict.Add("0765.jpg", "uve");
            _wordsMapApproxDict.Add("0766.jpg", "uve");
            _wordsMapApproxDict.Add("0767.jpg", "val");
            _wordsMapApproxDict.Add("0768.jpg", "val");
            _wordsMapApproxDict.Add("0769.jpg", "van");
            _wordsMapApproxDict.Add("0770.jpg", "van");
            _wordsMapApproxDict.Add("0771.jpg", "van");
            _wordsMapApproxDict.Add("0772.jpg", "var");
            _wordsMapApproxDict.Add("0773.jpg", "vas");
            _wordsMapApproxDict.Add("0774.jpg", "ved");
            _wordsMapApproxDict.Add("0775.jpg", "vej");
            _wordsMapApproxDict.Add("0776.jpg", "vej");
            _wordsMapApproxDict.Add("0777.jpg", "vel");
            _wordsMapApproxDict.Add("0778.jpg", "vel");
            _wordsMapApproxDict.Add("0779.jpg", "ver");
            _wordsMapApproxDict.Add("0780.jpg", "ver");
            _wordsMapApproxDict.Add("0781.jpg", "vid");
            _wordsMapApproxDict.Add("0782.jpg", "vid");
            _wordsMapApproxDict.Add("0783.jpg", "vil");
            _wordsMapApproxDict.Add("0784.jpg", "vil");
            _wordsMapApproxDict.Add("0785.jpg", "vil");
            _wordsMapApproxDict.Add("0786.jpg", "vin");
            _wordsMapApproxDict.Add("0787.jpg", "vis");
            _wordsMapApproxDict.Add("0788.jpg", "vis");
            _wordsMapApproxDict.Add("0789.jpg", "vov");
            _wordsMapApproxDict.Add("0790.jpg", "vov");
            _wordsMapApproxDict.Add("0791.jpg", "væg");
            _wordsMapApproxDict.Add("0792.jpg", "væg");
            _wordsMapApproxDict.Add("0793.jpg", "vær");
            _wordsMapApproxDict.Add("0794.jpg", "vær");
            _wordsMapApproxDict.Add("0795.jpg", "xyl");
            _wordsMapApproxDict.Add("0796.jpg", "yac");
            _wordsMapApproxDict.Add("0797.jpg", "æde");
            _wordsMapApproxDict.Add("0798.jpg", "æde");
            _wordsMapApproxDict.Add("0799.jpg", "æde");
            _wordsMapApproxDict.Add("0800.jpg", "ærg");
            _wordsMapApproxDict.Add("0801.jpg", "æri");
            _wordsMapApproxDict.Add("0802.jpg", "øje");
            _wordsMapApproxDict.Add("0803.jpg", "øjn");
            _wordsMapApproxDict.Add("0804.jpg", "øst");
            _wordsMapApproxDict.Add("0805.jpg", "øst");
            _wordsMapApproxDict.Add("0806.jpg", "ånd");
            _wordsMapApproxDict.Add("0807.jpg", "ånd");
        }

        #endregion

        internal class WordToPageMap
        {
            public WordToPageMap(string fileName, string firstWord, string lastWord)
            {
                FileName = fileName;
                FirstWord = firstWord;
                LastWord = lastWord;
            }

            public string FirstWord { get; private set; }

            public string LastWord { get; private set; }

            public string FileName { get; private set; }
        }
    }
}
