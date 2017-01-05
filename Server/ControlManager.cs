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
        static GPIOMem _pinLeftEngineForward = new GPIOMem(GPIOPins.Pin_P1_23);
        static GPIOMem _pinLeftEngineBackward = new GPIOMem(GPIOPins.Pin_P1_21);
        static GPIOMem _pinRightEngineForward = new GPIOMem(GPIOPins.Pin_P1_24);
        static GPIOMem _pinRightEngineBackward = new GPIOMem(GPIOPins.Pin_P1_26);

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
                case Global.CameraUp:
                    MoveUp();
                    break;
                case Global.CameraDown:
                    MoveDown();
                    break;
                case Global.CameraLeft:
                    MoveLeft();
                    break;
                case Global.CameraRight:
                    MoveRight();
                    break;
                case Global.Forward:
                    MoveForward();
                    break;
                case Global.Backward:
                    MoveBackward();
                    break;
                case Global.TurnLeft:
                    TurnLeft();
                    break;
                case Global.TurnRight:
                    TurnRight();
                    break;
            }
        }

        private void TurnRight()
        {
            _pinLeftEngineForward.Write(true);
            _pinRightEngineBackward.Write(true);
            Thread.Sleep(200);
            _pinLeftEngineForward.Write(false);
            _pinRightEngineBackward.Write(false);
            Log("Turn right");
        }

        private void TurnLeft()
        {
            _pinLeftEngineBackward.Write(true);
            _pinRightEngineForward.Write(true);
            Thread.Sleep(200);
            _pinLeftEngineBackward.Write(false);
           _pinRightEngineForward.Write(false);
            Log("Turn left");
        }

        private void MoveBackward()
        {
            _pinLeftEngineBackward.Write(true);
            _pinRightEngineBackward.Write(true);
            Thread.Sleep(800);
            _pinLeftEngineBackward.Write(false);
            _pinRightEngineBackward.Write(false);
            Log("Turn right");
        }

        private void MoveForward()
        {
            _pinLeftEngineForward.Write(true);
            _pinRightEngineForward.Write(true);
            Thread.Sleep(800);
            _pinLeftEngineForward.Write(false);
            _pinRightEngineForward.Write(false);
            Log("Turn right");
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