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
    public delegate void NewTicketEvent(NewTicketEventArgs e);
    public delegate void TicketServiceBreakEvent(EventArgs e);
    public delegate void TicketServiceTransactionCompleteEvent(EventArgs e);

    public class TicketServiceClient : ITicketServiceClient
    {
        public event NewTicketEvent NewTicketEventHandler;
        public event TicketServiceBreakEvent TicketServiceBreakEventHandler;
        public event TicketServiceTransactionCompleteEvent TicketServiceTransactionCompleteEventHandler;

        private enum RequestState
        {
            NotReady = 0,
            Ready = 1,
            TicketReady,
            TicketUserRecognized, 
            Ticket,
            Complete,
            Break,
            Disconnected
        };

        private RequestState _requestState = RequestState.NotReady;

        private static readonly object Lock = new object();
        private static TicketServiceClient _instance;

        private bool _waitForTicket = true;
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
            _waitForTicket = false;
            _connectionProxy.Disconnect();
            _requestState = RequestState.Disconnected;

            _connectionProxy.Dispose();
            _connectionProxy = null;
        }



        private void ValidateUser(string user, string password, string extension)
        {
            // need to convert to msg command
            string reqMsg = Parser.GetValidateUserCommand(user);


            //string resp = _connectionProxy.Send(msg);
            var resp = _connectionProxy.Send(reqMsg);

            // validate the response
            bool valid = Parser.UserExists(resp);
            if (!valid)
            {
                throw new InvalidCredentialException($"Invalid user {user}");
            }

        }

        private void Login(string user, string password, string extension)
        {
            string reqMsg = Parser.GetLoginCommand(user);

            var resp = _connectionProxy.Send(reqMsg);

            bool success = Parser.LoginSuccessful(resp);
            if (!success)
            {
                throw new InvalidCredentialException($"The user {user} failed to login.");
            }

            User = user;

            _requestState = RequestState.Ready;
            Task.Run(() => StartListener() );
        }


        public void CompleteTransaction(int outcomeCode)
        {
            ValidateState(RequestState.Complete);
            var msg = Parser.GetTransactionCompleteCommand(User, outcomeCode);
            _requestState = RequestState.Complete;
            _connectionProxy.SendAsync(msg);

            
        }

        public void RequestBreak()
        {
            ValidateState(RequestState.Break);

            var msg = Parser.GetBreakRequestCommand(User);
            _requestState = RequestState.Break;
            var resp = _connectionProxy.Send(msg);
            
        }

        public void Ready()
        {
            ValidateState(RequestState.TicketReady);
            // send get ready
            var msg = Parser.GetReadyCommand(User);
            _requestState = RequestState.TicketReady;
            _connectionProxy.SendAsync(msg);
        }

        private void StartListener()
        {
            _requestState = RequestState.Ready;
            while (_waitForTicket)
            {
                var msgs = _connectionProxy.ReadAsync();
                foreach (var msg in msgs)
                {
                    switch (_requestState)
                    {
                        case RequestState.Ticket:
                        case RequestState.TicketReady:
                        case RequestState.TicketUserRecognized:
                            HandleTicketResponse(msg);
                            break;

                        case RequestState.Break:
                            HandleRequestBreakResponse(msg);
                            break;

                        case RequestState.Complete:
                            HandleTransactionCompleteResponse(msg);
                            break;
                    }
                }

            }
        }

        private void HandleTicketResponse(string msg)
        {
            switch (_requestState)
            {
                case RequestState.TicketReady:
                    if (!Parser.IsReadySuccessful(msg))
                        throw new InvalidOperationException("Ready failed!");
                    _requestState = RequestState.TicketUserRecognized;
                    break;

                case RequestState.TicketUserRecognized:
                    if (!Parser.IsUserRecongnizedReady(msg))
                        throw new InvalidOperationException("User not recognized as ready!");

                    _requestState = RequestState.Ticket;
                    break;

                default:
                    if (NewTicketEventHandler != null)
                    {
                        NewTicketEventHandler(new NewTicketEventArgs(Parser.GetTicket(msg)));
                    }

                    break;
            }
        }

        private void HandleRequestBreakResponse(string msg)
        {
            if (!Parser.BreakGranted(msg))
                throw new Exception("RequestBreak failed!");

            _waitForTicket = false;

            if (TicketServiceBreakEventHandler != null)
            {
                TicketServiceBreakEventHandler(EventArgs.Empty);
            }
        }

        private void HandleTransactionCompleteResponse(string msg)
        {
            if (!Parser.TransactionCompleted(msg))
                throw new Exception("CompleteTransaction failed!");

            _waitForTicket = false;

            if (TicketServiceTransactionCompleteEventHandler != null)
            {
                TicketServiceTransactionCompleteEventHandler(EventArgs.Empty);
            }
        }

        private void ValidateState(RequestState newState)
        {
            switch (newState)
            {
                case RequestState.TicketReady:
                    if (_requestState != RequestState.Ready)
                    {
                        throw new InvalidOperationException(
                            $"Invalid state to receive ticket, {_requestState.ToString()}");
                    }
                    while (_requestState == RequestState.NotReady)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    break;

                case RequestState.Break:
                    if (_requestState == RequestState.Break)
                    {
                        throw new InvalidOperationException(
                            $"Invalid state to receive ticket, {_requestState.ToString()}");
                    }

                    while (_requestState == RequestState.NotReady ||
                           _requestState == RequestState.TicketReady || 
                           _requestState == RequestState.TicketUserRecognized)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    break;

                case RequestState.Complete:
                    if (_requestState == RequestState.Break || 
                        _requestState == RequestState.Complete)
                    {
                        throw new InvalidOperationException(
                            $"Invalid state to receive ticket, {_requestState.ToString()}");
                    }

                    while (_requestState == RequestState.NotReady ||
                           _requestState == RequestState.TicketReady ||
                           _requestState == RequestState.TicketUserRecognized)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    break;
            }
        }
    }
}
