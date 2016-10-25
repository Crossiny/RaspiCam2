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
        protected TcpClient Client;
        protected TcpListener Listener;
        protected bool Running;
        protected int SleepTime = 50;
        protected NetworkStream Stream;
        protected Task Task;

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
        public TaskStatus Status => Task.Status;

        protected void Start()
        {
            // Starts a new Task that accepts clients.
            Task = new Task(() =>
            {
                Listener.Start();
                Log("Listener started.");
                while (Running)
                {
                    // Short break to reduce load while nothing important happens.
                    Thread.Sleep(SleepTime);

                    // Ony accepts a new connection if no client is connected
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

                // Stops the listener if thread is about to finish.
                Listener.Stop();
                Log("Listener stopped");
            });
            Task.Start();
            Log("Task started.");
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
            if (readByte == Global.Message) ProcessMessage();
            if (readByte == Global.CheckConnection) ProcessConnectionCheck();
            if (readByte == Global.CloseConnection) ProcessCloseConnection();
        }

        [DebuggerStepThrough]
        protected void Log(string message)
        {
            Console.WriteLine($"{Identifier} - {message}");
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
                var c = (char) Stream.ReadByte();
                if (c == Global.EOT) break;
                message += c;
            }
            Log(message);
        }
    }
}