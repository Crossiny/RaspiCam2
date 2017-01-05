using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal abstract class Manager
    {
        protected bool Running;
        protected int SleepTime = 50;
        protected Task Task;
        protected TcpClient Client;
        protected TcpListener Listener;
        protected NetworkStream Stream;

        protected Manager(IPEndPoint ipEndPoint)
        {
            Running = true;
            Listener = new TcpListener(ipEndPoint);
            Start();
        }

        protected abstract string Identifier { get; }

        /// <summary>
        ///     Returns the status of the task executing the connection.
        /// </summary>
        public TaskStatus Status { get { return Task.Status; } }

        protected void Start()
        {
            // Starts a new Task that accepts clients.
            Task = new Task(() =>
            {
                Listener.Start();
                Log("Listener started.");
                RunLoop();

                // Stops the listener if thread is about to finish.
                Listener.Stop();
                Log("Listener stopped");
            });
            Task.Start();
            Log("Task started.");
        }

        private void RunLoop()
        {
            while (Running)
            {
                // Short break to reduce load while nothing important happens.
                Thread.Sleep(SleepTime);

                // Only accepts a new connection if no client is connected
                if (Listener.Pending() && ((Client == null) || !Client.Connected))
                {
                    Client = Listener.AcceptTcpClient();
                    Stream = Client.GetStream();
                    Log("Client Connected.");
                }

                // Only executes if the client is connected and data is available, otherwise Stream.ReadByte() would block.
                if ((Client != null) && Client.Connected && Stream.DataAvailable)
                    ProcessCommand((byte) Stream.ReadByte());
            }
        }

        /// <summary>
        ///     Closes the connection and stops the listener.
        /// </summary>
        public void Close()
        {
            // Causes the Task to finish and clean up.
            Running = false;
        }

        [DebuggerStepThrough]
        protected virtual void ProcessCommand(byte readByte)
        {
            switch (readByte)
            {
                case Global.Message:
                    ProcessMessage();
                    break;
                case Global.CheckConnection:
                    ProcessConnectionCheck();
                    break;
                case Global.CloseConnection:
                    ProcessCloseConnection();
                    break;
            }
        }

        [DebuggerStepThrough]
        protected void Log(string message)
        {
            Console.WriteLine("{0} - {1}",Identifier,message);
        }

        private void ProcessConnectionCheck()
        {
            Stream.WriteByte(Global.CheckConnection);
        }

        private void ProcessCloseConnection()
        {
            Log("Connection closed.");
            Client.Close();
        }

        private void ProcessMessage()
        {
            var message = "";
            while (true)
            {
                var c = (char)Stream.ReadByte();
                if (c == Global.EOT) break;
                message += c;
            }
            Log(message);
        }
    }
}