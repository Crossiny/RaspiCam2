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
            switch (readByte)
            {
                case Global.RequestImage:
                    try
                    {
                        GenerateImage(hFlip: true);
                        SendImage(Stream);
                    }
                    catch (IOException e)
                    {
                        Log(e.Message);
                    }
                    break;
            }
        }

        public void GenerateImage(string path = "./image.jpg", bool nightvision = false, bool hFlip = false, bool vFlip = false, int width = 600, int height = 600, int timer = 1)
        {
            // Builds parameter to flip the image if needed.
            string paramString = "raspistill ";
            paramString += $@"-o ""{path}"" ";
            paramString += $@"-t {timer} ";
            paramString += $@"-w {width} ";
            paramString += $@"-h {height} ";
            paramString += hFlip ? "-hf " : "";
            paramString += vFlip ? "-vf " : "";
            paramString += nightvision ? "--exposure nightpreview" : "";
            // Creates and starts a process that generates the image form the camera.
            Process raspistillProcess = new Process
            {
                StartInfo = new ProcessStartInfo(paramString)
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