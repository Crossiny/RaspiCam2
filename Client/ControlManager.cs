namespace Client
{
    internal class ControlManager : Manager
    {
        public ControlManager(string hostname, int port) : base(hostname, port)
        {
        }

        public void MoveCamera(byte direction)
        {
            Stream.WriteByte(direction);
        }
        public void Move(byte direction)
        {
            Stream.WriteByte(direction);
        }
    }
}