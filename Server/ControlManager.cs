using System.Net;

namespace Server
{
    internal class ControlManager : Manager
    {
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
            // TODO: Implement MoveUp().
            Log("Moves up");
        }

        protected void MoveDown()
        {
            // TODO: Implement MoveDown().
            Log("Moves down");
        }

        protected void MoveLeft()
        {
            // TODO: Implement MoveLeft().
            Log("Moves left");
        }

        protected void MoveRight()
        {
            // TODO: Implement MoveRight().
            Log("Moves right");
        }
    }
}