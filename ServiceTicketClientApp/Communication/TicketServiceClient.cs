using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Communication
{
    public class TicketServiceClient: ITicketServiceClient
    {
        private static readonly object Lock = new object();
        private static TicketServiceClient _instance;
        private ConcurrentQueue<TicketMessage> concurrentQueque = new ConcurrentQueue<TicketMessage>();

        private RemoteClientProxy _connectionProxy;

        public string User { get; set; }

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
            // need to convert to msg command
            string reqMsg = Parser.GetValidateUserCommand(user);


            //string resp = _connectionProxy.Send(msg);
            var resp = _connectionProxy.Send(reqMsg);

            // validate the response
            bool valid = Parser.UserExists(resp.FirstOrDefault());
            if (!valid)
            {
                throw new InvalidCredentialException($"Invalid user {user}");
            }

        }

        private void Login(string user, string password, string extension)
        {

            string reqMsg = Parser.GetLoginCommand(user);

            var resp = _connectionProxy.Send(reqMsg);

            bool success = Parser.LoginSuccessful(resp.FirstOrDefault());
            if (!success)
            {
                throw new InvalidCredentialException($"The user {user} failed to login.");
            }

            User = user;
        }

        

        public void GetTicketsAsync(string user,AsyncCallback ticketReady)
        {
            // send get ready
            var msg = Parser.GetReadyCommand(user);
            var resp = _connectionProxy.Send(msg,2);



            //_connectionProxy.GetReady();

            Task.Run(() =>
            {
                // get tickets
                //string[] data = _connectionProxy.WaitForIncomingData();
            });

            // ticket callback
        }

        public void CompleteTransaction(int OutcomeCode)
        {
            var msg = Parser.GetTransactionCompleteCommand(User, OutcomeCode);
            var resp = _connectionProxy.Send(msg);
            if (!Parser.TransactionCompleted(resp.First()))
                throw new Exception("CompleteTransaction failed!");

            // transaction complete
            _connectionProxy.Complete();
        }

        public void RequestBreak()
        {
            // request a break from server
            _connectionProxy.RequestBreak();
        }

        public void Ready()
        {
            Task.Run(() =>
            {
                // send get ready
                var msg = Parser.GetReadyCommand(User);
                var resp = _connectionProxy.Send(msg, 2);

                // get tickets
                //string[] data = _connectionProxy.WaitForIncomingData();
            });


        }

        public TicketMessage GetTicketMessage()
        {
            TicketMessage msg;
            while (!concurrentQueque.TryDequeue(out msg)) ;
            return msg;
        }
    }
}
