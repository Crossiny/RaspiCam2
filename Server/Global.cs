namespace Server
{
    public class Global
    {
        public const byte Forward = 0x0;            // Bewegung nach Vorne
        public const byte Backward = 0x1;           // Bewegung nach hinten
        public const byte TurnLeft = 0x2;           // Nach links drehen
        public const byte TurnRight = 0x3;          // Nach rechts drehen
        public const byte EOT = 0x4;                // Ende der Übertragung
        public const byte CheckConnection = 0x8;    // Überprüft ob der Server antwortet
        public const byte Message = 0x9;            // Beginn einer Nachricht, endet mit EOT
        public const byte CameraUp = 0xA;           // Kamera nach oben kippen
        public const byte CameraDown = 0xB;         // Kamera nach unten kippen
        public const byte CameraLeft = 0xC;         // Kamera nach links schwenken
        public const byte CameraRight = 0xD;        // Kamera nach rechts schwenken
        public const byte RequestImage = 0xE;       // Bild vom Server anfordern
        public const byte CloseConnection = 0xF;    // Verbindung beenden
    }
    
}