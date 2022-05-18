using Model;
using System.ComponentModel;

namespace ViewModels
{
    public class LoginPageViewModel
    {
        private string _password;
        private string _extension;
        private string _campaign;
        private string _userId;
        public string UserId 
        {     
            get
            {
                return _userId;
            }
            
            set
            {
                RaisePropertyChanged(nameof(UserId));
            } 
        }

        public string Password 
        { 
            get
            {
                return _password;
            }

            set
            {
                RaisePropertyChanged(Password);

            }
        }

        public string Extension
        { 
            get
            {
                return _extension;
            }

            set
            {
                RaisePropertyChanged(Extension);
            }
        
        }
        public string Campaign
        { 
            get
            {
                return _campaign;
            }
            set
            {
                RaisePropertyChanged(Campaign);
            }
        }
        
        public LoginPageViewModel()
        {

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
