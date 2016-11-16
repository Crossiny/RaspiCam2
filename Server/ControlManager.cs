using System.Net;
using System.Threading;
using RaspberryPiDotNet;

namespace Server
{
    internal class ControlManager : Manager
    {
        static GPIOMem _pinLeft = new GPIOMem(GPIOPins.Pin_P1_22);
        static GPIOMem _pinRight = new GPIOMem(GPIOPins.Pin_P1_07);
        static GPIOMem _pinUp = new GPIOMem(GPIOPins.Pin_P1_18);
        static GPIOMem _pinDown = new GPIOMem(GPIOPins.Pin_P1_16);

        public ControlManager(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
        }

        protected override string Identifier
        {
            get { return "ControlManager"; }
        }

        protected override void ProcessCommand(byte readByte)
        {
            base.ProcessCommand(readByte);

            switch (readByte)
            {
                case Global.MoveUp:
                    MoveUp();
                    break;
                case Global.MoveDown:
                    MoveDown();
                    break;
                case Global.MoveLeft:
                    MoveLeft();
                    break;
                case Global.MoveRight:
                    MoveRight();
                    break;
            }
        }

        protected void MoveUp()
        {
            _pinUp.Write(true);
            Thread.Sleep(100);
            _pinUp.Write(false);
            Log("move up");
        }

        protected void MoveDown()
        {
            // TODO: Implement MoveDown().
            _pinDown.Write(true);
            Thread.Sleep(100);
            _pinDown.Write(false);
            Log("move down");
        }

        protected void MoveLeft()
        {
            _pinLeft.Write(true);
            Thread.Sleep(400);
            _pinLeft.Write(false);
            Log("move left");
        }

        protected void MoveRight()
        {
            _pinRight.Write(true);
            Thread.Sleep(400);
            _pinRight.Write(false);
            Log("move right");
        }
    }
}