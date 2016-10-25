using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Server
{
    internal class ImageManager : Manager
    {
        public ImageManager(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
        }

        protected override string Identifier
        {
            get { return "ImageManager"; }
        }

        protected override void ProcessCommand(byte readByte)
        {
            base.ProcessCommand(readByte);
            if (readByte == Global.RequestImage)
                try
                {
                    // TODO: Uncoment GenerateImage().
                    // GenerateImage();

                    // FIXME: Set existing image path for testing.
                    SendImage(Stream, $@"./images/image ({new Random().Next(0, 7)}).jpg");
                }
                catch (IOException e)
                {
                    Log(e.Message);
                }
        }

        public void GenerateImage(string path = "./image.jpg", bool hFlip = false, bool vFlip = false)
        {
            // Builds parameter to flip the image if needed.
            var flipString = (hFlip ? " -hf" : "") + (vFlip ? " -vf" : "");

            // Creates and starts a process that generates the image form the camera.
            var raspistillProcess = new Process
            {
                StartInfo = new ProcessStartInfo("raspistill", string.Format("-o \"{0}\"{1}", path, flipString))
            };
            raspistillProcess.Start();
            raspistillProcess.WaitForExit();
        }

        public void SendImage(Stream stream, string path = "./image.jpg")
        {
            // Gets the bytes from the image.
            var image = new byte[0];

            try
            {
                image = File.ReadAllBytes(path);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
            // Writes the Image to the stream and flushes the stream.
            Console.WriteLine($"Sending image, {image.Length} Bytes...");
            stream.Write(image, 0, image.Length);

            // Bytes that indicate the end of the current transmission.
            stream.Write(new byte[] { 0xFA, 0xAF, 0xFA, 0xAF }, 0, 4);
            Console.WriteLine("Image sent.");
        }
    }
}