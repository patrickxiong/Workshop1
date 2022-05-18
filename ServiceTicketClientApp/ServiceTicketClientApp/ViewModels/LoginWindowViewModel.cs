﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ServiceTicketClientApp.ViewModels
{
    public class LoginWindowViewModel
    {
        private bool _isLoggedIn;
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
                _userId = value;
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
                _password = value;
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
                _extension = value;
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
                _campaign = value;
                RaisePropertyChanged(Campaign);
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                _isLoggedIn = value;
                RaisePropertyChanged((nameof(IsLoggedIn)));
            }
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
