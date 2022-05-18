using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class RemoteClientProxy : IDisposable
    {
        private const int PollTime = 1000; // 1 second
        private const int BufferSize = 1024;
        private Socket _client;
        private bool _disposedValue;

        public bool IsConnected
        {
            get
            {
                if (_client == null)
                {
                    return false;
                }

                bool pollSuccess = _client.Poll(PollTime, SelectMode.SelectRead);

                return pollSuccess;
            }
        }

        public bool IsReady => IsConnected && _client.Available == 0;

        public bool Connect(string server, int port)
        {
            try
            {
                IPAddress ipAddr = IPAddress.Parse(server);
                IPEndPoint remoteEp = new IPEndPoint(ipAddr, port);


                _client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                _client.Connect(remoteEp);

                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void Disconnect()
        {
            if (_client != null)
            {
                try
                {
                    _client.Shutdown(SocketShutdown.Both);
                    _client.Close();
                    _client = null;
                }
                catch
                {
                    //
                }
            }
        }

        public string Send(string message, bool withReply = true)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            try
            {
                List<byte> buffer = new List<byte>();
                byte[] byteData = Encoding.ASCII.GetBytes(message);

                int offset = 0;
                while (true)
                {
                    int bytesSent = _client.Send(byteData, offset, BufferSize, SocketFlags.None);
                    offset += bytesSent;
                    if (bytesSent < BufferSize || offset >= byteData.Length)
                    {
                        break;
                    }
                }

                //int bytesSent = _client.Send(byteData);

                if (withReply)
                {
                    offset = 0;

                    while (true)
                    {
                        byte[] receiveBuffer = new byte[BufferSize];
                        int size = _client.Receive(receiveBuffer, offset, BufferSize, SocketFlags.None);
                        if (size > 0)
                        {
                            buffer.AddRange(receiveBuffer);
                            offset += size;
                        }

                        if (size < BufferSize)
                        {
                            break;
                        }
                    }
                }

                string response = string.Empty;
                if (buffer.Count > 0)
                {
                    response = Encoding.ASCII.GetString(buffer.ToArray(), 0, buffer.Count);
                }

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Disconnect();
                }

                _disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
