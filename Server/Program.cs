using System;
using System.Net;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Manager imageManager = new ImageManager(new IPEndPoint(IPAddress.Any, 1337));
            Manager controlManager = new ControlManager(new IPEndPoint(IPAddress.Any, 1338));

            /*
            TcpClient client = new TcpClient("localhost", 1337);
            NetworkStream stream = client.GetStream();
            stream.WriteByte(Global.Message);
             */

            Console.Read();
        }
    }
}