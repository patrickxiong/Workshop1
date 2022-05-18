using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class TicketServiceClient
    {
        private static readonly object Lock = new object();
        private static TicketServiceClient _instance;

        private RemoteClientProxy _connectionProxy;

        private ConcurrentQueue<TicketMessage> concurrentQueque = new ConcurrentQueue<TicketMessage>();

        private TicketServiceClient()
        {

        }

        public static TicketServiceClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TicketServiceClient();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Connect(NetworkConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (_connectionProxy != null && _connectionProxy.IsConnected)
            {
                _connectionProxy.Disconnect();
            }

            _connectionProxy = new RemoteClientProxy();

            _connectionProxy.Connect(config.Server, config.Port);

            ValidateUser(config.User, config.Password, config.Extension);

            Login(config.User, config.Password, config.Extension);
        }

        public void Disconnect()
        {

        }



        private void ValidateUser(string user, string password, string extension)
        {
            var msg = Parser.GetValidateUserCommand(user);

            //string resp = _connectionProxy.Send(msg);
            string resp = _connectionProxy.Send(msg);

            // validate the response
            if (!Parser.UserExists(resp))
                throw new Exception("User does not exist!");
        }

        private void Login(string user, string password, string extension)
        {
            var msg = Parser.GetLoginCommand(user);

            string resp = _connectionProxy.Send(msg);

            if (!Parser.LoginSuccessful(resp))
                throw new Exception("Login failed!");
        }

        public void Ready(string user)
        {
            Task.Run(() =>
            {
                // send get ready
                var msg = Parser.GetReadyCommand(user);
                string resp = _connectionProxy.Send(msg, 2);

                // get tickets
                //string[] data = _connectionProxy.WaitForIncomingData();
            });


        }

        public TicketMessage GetTicketMessage()
        {
            TicketMessage msg;
            while(!concurrentQueque.TryDequeue(out msg));
            return msg;
        }

        public void GetTicketsAsync(string user, AsyncCallback ticketReady)
        {
            // send get ready
            var msg = Parser.GetReadyCommand(user);
            string resp = _connectionProxy.Send(msg, 2);



            //_connectionProxy.GetReady();

            Task.Run(() =>
            {
                // get tickets
                //string[] data = _connectionProxy.WaitForIncomingData();
            });

            // ticket callback
        }

        public void CompleteTransaction()
        {
            // transaction complete
            _connectionProxy.Complete();
        }

        public void RequestBreak()
        {
            // request a break from server
            _connectionProxy.RequestBreak();
        }
    }
}
