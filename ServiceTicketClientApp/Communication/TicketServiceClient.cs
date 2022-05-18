using System;
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
            string msg = $"UA\\AN{user}\\TDdefault";

            //string resp = _connectionProxy.Send(msg);
            string resp = _connectionProxy.Send(msg);

            // validate the response
        }

        private void Login(string user, string password, string extension)
        {
            // need to convert to msg command
            string msg = $"AL\\AN{user}\\AE{password}\\AD{user}\\CN{extension}\\TDdefault";

            string resp = _connectionProxy.Send(msg);
        }

        

        public void GetTicketsAsync(AsyncCallback ticketReady)
        //public void GetTicketsAsync()
        //public void Ready(Ia)
        {
            // send get ready
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
