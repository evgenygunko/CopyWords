using System;
using System.Windows.Input;

namespace CopyWordsWPF.ViewModel.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
