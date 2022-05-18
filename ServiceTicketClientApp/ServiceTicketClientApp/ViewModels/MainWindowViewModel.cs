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
        
        // Outcome as an enum;

        public string Campaign
        {
            get { return _campaign; }
            set { _campaign = value; }
        }
        public string UserId
        { 
            get { return _userId; } 
            private set { _userId = value; }
        }

        public string TicketId
        {
            get { return _ticketId; } 
            set { _ticketId = value; } 
        }
        public string Type
        {
            get { return _type; }
            private set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        { 
            get { return _email; } 
            set { _email = value; }
        }

        public MainWindowViewModel()
        {

        }
    }
}
