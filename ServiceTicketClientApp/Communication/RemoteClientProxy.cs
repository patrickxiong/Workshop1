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

                //bool pollSuccess = _client..Poll(PollTime, SelectMode.SelectRead);

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
                    
                    _client.Close();
                    _client = null;
                }
                catch
                {
                    //
                }
            }
        }

        public string Send(string message, int numberOfResp=1)
        {
            message = $"{message}\0";
            
            byte[] byteData = Encoding.ASCII.GetBytes(message);
            string resp;
            _client.ReceiveTimeout = 10000; // 10 seconds

            using (var stream = _client.GetStream())
            {
                stream.Write(byteData, 0, byteData.Length);

                byte[] respData = new byte[1024];
                int size = stream.Read(respData, 0, respData.Length);

                resp = System.Text.Encoding.ASCII.GetString(respData, 0, size);
            }


            return resp;
        }


        public string[] WaitForIncomingData()
        {
            return new string[3];
        }

        public void Complete()
        {

        }

        public void RequestBreak()
        {

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
