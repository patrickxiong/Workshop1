using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication
{
    public static class TcpOptions
    {
        public static char EndMessageCode { get; set; } = (char)0;
        public static char EndConnectionCode { get; set; } = (char)4;
    }
    public class ResponseEvent
    {
        public string Content { get; set; }
        public ContentMode Mode { get; set; }
        public Action<MessageReceivedEventArgs> Event { get; set; }
    }
    public interface ITimed
    {
        /// <summary>
        /// The time the event happened.
        /// </summary>
        DateTime Time { get; set; }
    }
    public class MessageReceivedEventArgs : EventArgs, ITimed
    {
        /// <summary>
        /// The time the event happened.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The message that was sent.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The Client that sent the message.
        /// </summary>
        public Client Client { get; set; }
    }
    public enum ContentMode
    {
        Contains,
        StartsWith,
        EndsWish,
        Equals,
    }
    public class Client
    {
        TcpClient _client;
        NetworkStream _stream => _client.GetStream();
        Thread _listenThread;

        /// <summary>
        /// Whether the Client has been disconnected and disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// When a message is received from the server this client is connected to.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// When the server stops, disposing the client automatically.
        /// </summary>
        public event EventHandler ServerStopped;

        /// <summary>
        /// When the client is disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// When the client has begun listening for messages from the server.
        /// </summary>
        public event EventHandler StartedListening;

        /// <summary>
        /// If this is the client end creating the connected, this is false.
        /// If this is the client the sever creates on the server end after a client has connected, this is true.
        /// </summary>
        public bool IsServerClient { get; }

        /// <summary>
        /// If the instantiation, and inheritly the connection, failed.
        /// </summary>
        public bool FailedConnect { get; }

        /// <summary>
        /// The tag attached to this object.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// The endpoint of the client. If <see cref="IsServerClient"/>, this returns the originating client's IP endpoint. 
        /// If not true, returns the address of the server.
        /// </summary>
        public string ConnectAddress => IPAddress.Parse(((IPEndPoint)_client.Client.RemoteEndPoint).Address.ToString()).ToString();

        /// <summary>
        /// The port this client is connected to the server on.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// If it's server-side.
        /// </summary>
        /// <param name="client"></param>
        public Client(TcpClient client)
        {
            _client = client;
            IsServerClient = true;
            Port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;

            StartListening();
        }

        /// <summary>
        /// If it's client side.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public Client(string address, int port)
        {
            try
            {
                Port = port;
                _client = new TcpClient(address, port);

                StartListening();
            }
            catch
            {
                FailedConnect = true;
            }
        }

        /// <summary>
        /// Starts the client listening for messages.
        /// </summary>
        private void StartListening()
        {
            _listenThread = new Thread(ListenForMessages);
            _listenThread.Start();
            StartedListening?.Invoke(this, null);
        }

        /// <summary>
        /// Sends a message to the endpoint of this client.
        /// </summary>
        /// <param name="content"></param>
        public void SendMessage(object content)
        {
           var message = $"{content}\0";

            var outContent = content.ToString();
            //outContent += TcpOptions.EndMessageCode.ToString();
            //var data = outContent.GetBytes();

            byte[] data = Encoding.ASCII.GetBytes(message);

            _stream.Write(data, 0, data.Length);
        }


        /// <summary>
        /// The thread method where the client listens for new messages and handles them accordingly.
        /// </summary>
        private void ListenForMessages()
        {
            var bytes = new List<byte>();

            while (!IsDisposed)
            {
                var i = -1;

                try
                {
                    i = _stream.ReadByte();
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.Interrupted)
                    {
                        break;
                    }
                }
                catch
                {
                    break;
                }

                if (i == -1)
                {
                    break;
                }
                else if (i == TcpOptions.EndMessageCode)
                {
                    if (bytes.Count > 0)
                    {
                        var message = System.Text.Encoding.ASCII.GetString(bytes.ToArray(), 0, bytes.Count);

                        var eventargs = new MessageReceivedEventArgs
                        {
                            Message = message,
                            Time = DateTime.Now,
                            Client = this
                        };
                        MessageReceived?.Invoke(this, eventargs);
                        bytes.Clear();
                    }
                }
                else if (i == TcpOptions.EndConnectionCode && !IsServerClient)
                {
                    ServerStopped?.Invoke(this, null);
                    Dispose(true);
                    break;
                }
                else
                {
                    bytes.Add(Convert.ToByte(i));
                }
            }
        }

        public void Disconnect()
        {
            _client.Close();
        }

        /// <summary>
        /// Stops the client from listening, sending an end connection code to the server, and disposing.
        /// </summary>
        /// <param name="fromServer"></param>
        public void Dispose(bool fromServer)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                if (!fromServer)
                {
                   // SendMessage(TcpOptions.EndConnectionCode.ToString(), true);
                }
                _client.Close();
                _client.Dispose();
                _listenThread.Abort();
                Disposed?.Invoke(this, null);
            }
        }
    }
}
