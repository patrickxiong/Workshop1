﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class ServiceClient : IServiceClient
    {
        private static readonly object Lock = new object();
        private static ServiceClient _instance;
        private Client _connectionProxy;
        private ConcurrentQueue<TicketMessage> concurrentQueque = new ConcurrentQueue<TicketMessage>();

        public string User { get; set; }
        private ServiceClient()
        {

        }

        public static ServiceClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ServiceClient();
                        }
                    }
                }

                return _instance;
            }
        }

        public void CompleteTransaction(int OutcomeCode)
        {
            throw new NotImplementedException();
        }

        public void Connect(NetworkConfiguration config)
        {
            if (_connectionProxy != null)
            {
                _connectionProxy.Disconnect();
            }

            _connectionProxy = new Client(config.Server,config.Port);
            _connectionProxy.MessageReceived += _connectionProxy_MessageReceived;

            ValidateUser(config.User, config.Password, config.Extension);

            Login(config.User, config.Password, config.Extension);

        }

        private void Login(string user, string password, string extension)
        {
            string reqMsg = Parser.GetLoginCommand(user);

            _connectionProxy.SendMessage(reqMsg);

            User = user;
        }


        private void _connectionProxy_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var t = Parser.GetTicket(e.Message);
            if(t != null)
                concurrentQueque.Enqueue(t);

        }

        private void ValidateUser(string user, string password, string extension)
        {
            // need to convert to msg command
            string reqMsg = Parser.GetValidateUserCommand(user);

            //string resp = _connectionProxy.Send(msg);
            _connectionProxy.SendMessage(reqMsg);
        }


        public void Disconnect()
        {
            _connectionProxy?.Disconnect();
        }

        public TicketMessage GetTicketMessage()
        {
            TicketMessage msg;

            while (!concurrentQueque.TryDequeue(out msg)) ;
            return msg;
        }

        public void Ready()
        {
            var msg = Parser.GetReadyCommand(User);
            _connectionProxy.SendMessage(msg);
        }

        public void RequestBreak()
        {
            var msg = Parser.GetBreakRequestCommand(User);
            
            _connectionProxy.SendMessage(msg);
        }
    }
}
