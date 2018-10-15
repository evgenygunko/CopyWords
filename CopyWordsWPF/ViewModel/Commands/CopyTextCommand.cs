using System.Windows;

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
