using System;
using System.Windows.Input;

namespace ServiceTicketClientApp.Command
{
    public class NextCommand : ICommand
    {
        Action _execute;
        public event EventHandler CanExecuteChanged;

        public NextCommand(Action nextFunction)
        {
            _execute = nextFunction;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
