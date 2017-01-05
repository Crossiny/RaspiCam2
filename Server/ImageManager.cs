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
                            GenerateImage(vFlip: true);
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
            string paramString = " -o " + path + " -t 1 -w 500 -h 500 -q 70 ";
            paramString += hFlip ? " -hf " : "";
            paramString += vFlip ? " -vf " : "";
            Console.WriteLine(paramString);
            // Creates and starts a process that generates the image form the camera.
            Process raspistillProcess = new Process
            {
                StartInfo = new ProcessStartInfo("raspistill", paramString)
            };
            raspistillProcess.Start();
            raspistillProcess.WaitForExit();
        }

        public void SendImage(Stream stream, string path = "./image.jpg")
        {
            byte[] image = new byte[0];
            image = File.ReadAllBytes(path);

            Console.WriteLine("Sending image, {1} Bytes...");
            stream.Write(image, 0, image.Length);
            
            stream.Write(new byte[] { 0xFA, 0xAF, 0xFA, 0xAF }, 0, 4);
            Console.WriteLine("Image sent.");
            stream.Flush();
        }
    }
}