using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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
            DataContextChanged += WordView_DataContextChanged;
        }

        private void WordView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _wordViewModel = e.NewValue as WordViewModel;
            _wordViewModel.FileCopied += _wordViewModel_FileCopied;
        }

        private void _wordViewModel_FileCopied(object sender, EventArgs e)
        {
            BeginStoryboard sb = Resources["FileCopiedStoryBoard"] as BeginStoryboard;
            sb.Storyboard.Begin();
        }
    }
}
