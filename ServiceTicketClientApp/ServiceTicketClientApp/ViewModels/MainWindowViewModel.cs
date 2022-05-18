using Communication;
using ServiceTicketClientApp.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ServiceTicketClientApp.ViewModels
{
    public class MainWindowViewModel
    {

        private ITicketServiceClient _serviceClient;
        private string _campaign;
        private string _userId;
        private string _ticketId;
        private string _type;
        private string _name;
        private string _email;
        private string _selectedOutcome;
        private List<string> _outcomeSource = new List<string> { "1", "2", "3" };
        public ICommand RequestBreak { get; }
        public ICommand RequestNext { get; }

        public string SelectedOutcome { 
            get
            {
                return _selectedOutcome;
            }
            set
            {
                _selectedOutcome = value;
            }
        }
        public List<string> OutcomeSource
        {
            get
            {
                return _outcomeSource;
            }
        }
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

        public MainWindowViewModel()
        {
            _serviceClient = TicketServiceClient.Instance;
            RequestBreak = new RelayCommand(async command => _serviceClient.RequestBreak());
            RequestNext = new RelayCommand(async command => await SetMessage());
        }

        public async Task SetMessage()
        { 
            var message = TicketServiceClient.Instance.GetTicketMessage();
            Type = message.TicketType;
            TicketId = message.TicketId;
            UserId = message.UserId;
            Campaign = message.CampaignName;
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

  
}
