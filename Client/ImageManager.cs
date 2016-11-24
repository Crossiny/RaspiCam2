using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Server;

namespace Client
{
    internal class ImageManager : Manager
    {
        public ImageManager(string hostname, int port) : base(hostname, port)
        {
        }

        [DebuggerStepThrough]
        public byte[] GetImage()
        {
            Stream.WriteByte(Global.RequestImage);
            Stream.Flush();

            byte[] image;
            var byteList = new List<byte>();

            while (true)
            {
                var buffer = Stream.ReadByte();
                if (buffer != -1) byteList.Add((byte)buffer);

                // If the last 4 bytes are 0xF, 0x0, 0xf, 0x0, end transmission and convert list to array.
                var length = byteList.Count;
                if ((length > 3) &&
                    (byteList[length - 1] == 0xAF) &&
                    (byteList[length - 2] == 0xFA) &&
                    (byteList[length - 3] == 0xAF) &&
                    (byteList[length - 4] == 0xFA))
                {
                    // Removes the 4 bytes that are used to indicate end of stream.
                    byteList.RemoveRange(length - 4, 4);
                    image = byteList.ToArray();
                    break;
                }
            }
            return image;
        }

        public ImageSource GetImage(byte[] image)
        {
            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(image);
                bitmapImage.EndInit();
                return bitmapImage;
            }
            catch
            {
                // TODO: Proper error handling.
                return null;
            }
        }
    }
}