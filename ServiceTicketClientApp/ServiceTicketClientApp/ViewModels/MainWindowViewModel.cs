using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ServiceTicketClientApp.ViewModels
{
    public class MainWindowViewModel
    {
        private string _campaign;
        private string _userId;
        private string _ticketId;
        private string _type;
        private string _name;
        private string _email;
        public ICommand RequestBreak { get; }
        public ICommand RequestNext { get; }
        // Outcome as an enum;

        public string Campaign
        {
            get { return _campaign; }
            set { 
                _campaign = value;
                RaisePropertyChanged(nameof(Campaign));
            }
        }
        public string UserId
        { 
            get { return _userId; } 
            set { 
                _userId = value;
                RaisePropertyChanged(nameof(UserId));
            }
        }

        public string TicketId
        {
            get { return _ticketId; } 
            set { 
                _ticketId = value;
                RaisePropertyChanged(nameof(TicketId));
            } 
        }
        public string Type
        {
            get { return _type; }
            set { 
                _type = value;
                RaisePropertyChanged(nameof(Type));
            }
        }

        public string Name
        {
            get { return _name; }
            set { 
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Email
        { 
            get { return _email; } 
            set { 
                _email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }

        private void Hello()
        {
            Console.Write("Hi");
        }
        public MainWindowViewModel()
        {
            Action h = Hello;
            RequestBreak = new BreakCommand(h);
            RequestNext = new NextCommand(h);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class BreakCommand : ICommand
    {
        Action _execute;
        public event EventHandler CanExecuteChanged;

        public BreakCommand(Action breakFunction)
        {
            _execute = breakFunction;
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

    public class NextCommand : ICommand
    {
        Action _execute;
        public event EventHandler CanExecuteChanged;

        public NextCommand(Action breakFunction)
        {
            _execute = breakFunction;
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
