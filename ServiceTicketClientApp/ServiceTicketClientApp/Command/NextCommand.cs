using Communication;
using System;
using System.Windows.Input;

namespace ServiceTicketClientApp.Command
{
    public class NextCommand : ICommand
    {
        private TicketMessage message;
        public event EventHandler CanExecuteChanged;

        public NextCommand(out TicketMessage nextFunction)
        {
            nextFunction = TicketServiceClient.Instance.GetTicketMessage();
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            message = TicketServiceClient.Instance.GetTicketMessage();
        }
    }
}
