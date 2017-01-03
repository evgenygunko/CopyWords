using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CopyWordsWPF.ViewModel;

namespace CopyWordsWPF.View
{
    /// <summary>
    /// Interaction logic for WordView.
    /// </summary>
    public partial class WordView : UserControl
    {
        private WordViewModel _wordViewModel;

        public WordView()
        {
            InitializeComponent();
            this.DataContextChanged += WordView_DataContextChanged;
        }

        private void WordView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _wordViewModel = e.NewValue as WordViewModel;
            _wordViewModel.FileCopied += _wordViewModel_FileCopied;
        }

        private void _wordViewModel_FileCopied(object sender, EventArgs e)
        {
            BeginStoryboard sb = this.Resources["FileCopiedStoryBoard"] as BeginStoryboard;
            sb.Storyboard.Begin();
        }
    }
}
