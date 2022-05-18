using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ServiceTicketClientApp.Command;

namespace ServiceTicketClientApp.ViewModels
{
    public class LoginWindowViewModel
    {
        private bool _isLoggedIn;
        private string _password;
        private string _extension;
        private string _campaign;
        private string _userId;
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ReadyCommand { get; }

        public LoginWindowViewModel()
        {
            LoginCommand = new RelayCommand(e => Login());
            LogoutCommand= new RelayCommand(e => Logout());
            ReadyCommand = new RelayCommand(e => Ready());
        }

        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                RaisePropertyChanged(nameof(UserId));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        public string Extension
        {
            get => _extension;
            set
            {
                _extension = value;
                RaisePropertyChanged(nameof(Extension));
            }

        }
        public string Campaign
        {
            get => _campaign;
            set
            {
                _campaign = value;
                RaisePropertyChanged(nameof(Campaign));
            }
        }

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                RaisePropertyChanged(nameof(IsLoggedIn));
            }
        }

        public void Login()
        {
            IsLoggedIn = true;
        }

        public void Logout()
        {
            IsLoggedIn = false;
        }

        public void Ready()
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
