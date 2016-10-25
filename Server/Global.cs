namespace Server
{
    public class Global
    {
        public const byte EOT = 0x4;
        public const byte CheckConnection = 0x8;
        public const byte Message = 0x9;
        public const byte MoveUp = 0xA;
        public const byte MoveDown = 0xB;
        public const byte MoveLeft = 0xC;
        public const byte MoveRight = 0xD;
        public const byte RequestImage = 0xE;
        public const byte CloseConnection = 0xF;
    }
}