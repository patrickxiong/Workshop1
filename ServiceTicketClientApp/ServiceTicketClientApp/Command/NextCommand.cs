﻿using System;
using System.Windows.Input;

namespace ServiceTicketClientApp.Command
{
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
