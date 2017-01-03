using System;
using System.Windows;
using System.Windows.Input;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class CopyTextCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            string text = parameter as string;
            Clipboard.SetText(text);           
        }        
    }
}
