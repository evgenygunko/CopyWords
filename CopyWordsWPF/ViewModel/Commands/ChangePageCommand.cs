using System;

namespace CopyWordsWPF.ViewModel.Commands
{
    public class ChangePageCommand : CommandBase
    {
        private Action _moveForward;
        private Action _moveBack;

        public ChangePageCommand(Action moveForward, Action moveBack)
        {
            _moveForward = moveForward;
            _moveBack = moveBack;
        }

        public override void Execute(object parameter)
        {
            bool moveNext = Convert.ToBoolean(parameter);

            if (moveNext)
            {
                _moveForward();
            }
            else
            {
                _moveBack();
            }
        }
    }
}
