using System.Net.Sockets;
using Server;

namespace Client
{
    internal abstract class Manager
    {
        protected TcpClient Client;
        protected NetworkStream Stream;

        protected Manager(string hostname, int port)
        {
            Client = new TcpClient(hostname, port);
            Stream = Client.GetStream();
        }

        public bool Connected
        {
            get
            {
                // False if client is not connected or null.
                if ((Client == null) || !Client.Connected) return false;
                try
                {
                    // True if server responses correctly.
                    Stream.WriteByte(Global.CheckConnection);
                    return Stream.ReadByte() == Global.CheckConnection;
                }
                catch
                {
                    // False if an error occurs while communicating with the server.
                    return false;
                }
            }
        }

        public void SendMessage(string message)
        {
            Stream.WriteByte(Global.Message);
            foreach (var c in message)
                Stream.WriteByte((byte) c);
            Stream.WriteByte(Global.EOT);
        }

        public void Disconnect()
        {
            Stream.WriteByte(Global.CloseConnection);
        }
    }
}