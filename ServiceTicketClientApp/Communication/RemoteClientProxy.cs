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
        private TcpClient _client;
        private bool _disposedValue;

        public bool IsConnected
        {
            get
            {
                if (_client == null)
                {
                    return false;
                }

                return _client.Connected;
            }
        }

        public bool IsReady => IsConnected && _client.Available == 0;

        public bool Connect(string server, int port)
        {
            try
            {
                IPAddress ipAddr = IPAddress.Parse(server);
                IPEndPoint remoteEp = new IPEndPoint(ipAddr, port);

                _client = new TcpClient(server, port);

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
                    
                    _client.Close();
                    _client = null;
                }
                catch
                {
                    //
                }
            }
        }

        public string Send(string message)
        {
            int offset = 0;
            List<byte> respBuffer = new List<byte>();

            // send the request
            SendAsync(message);

            var stream = _client.GetStream();
            while (true)
            {
                byte[] respData = new byte[1024];
                int size = stream.Read(respData, offset, respData.Length);

                if (size > 0)
                {
                    respBuffer.AddRange(respData);
                    offset += size;
                }

                if (size < BufferSize)
                {
                    string resp = System.Text.Encoding.ASCII.GetString(respBuffer.ToArray(), 0, respBuffer.Count);

                    return resp;
                }

            }
        }

        public void SendAsync(string message)
        {
            message = $"{message}\0";
            byte[] byteData = Encoding.ASCII.GetBytes(message);

            var stream = _client.GetStream();

            stream.Write(byteData, 0, byteData.Length);
        }



        public IEnumerable<string> ReadAsync()
        {
            var stream = _client.GetStream();

            int offset = 0;
            List<byte> respBuffer = new List<byte>();
            string[] msgs;
            while (true)
            {
                byte[] respData = new byte[1024];
                int size = stream.Read(respData, offset, respData.Length);

                if (size > 0)
                {
                    respBuffer.AddRange(respData);
                    offset += size;
                }

                if (size < BufferSize)
                {
                    string resp = System.Text.Encoding.ASCII.GetString(respBuffer.ToArray(), 0, respBuffer.Count);

                    msgs = SplitResponse(resp);


                    msgs = msgs.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    foreach (var msg in msgs)
                    {
                        yield return msg;
                    }

                    break;
                }

            }
        }

        private string[] SplitResponse(string respData)
        {
            return respData.Split('\0');
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
