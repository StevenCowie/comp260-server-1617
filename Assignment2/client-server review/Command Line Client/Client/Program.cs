using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class client
    {        
        static bool connected = false;

        static void clientReceiveThread(Object obj)
        {
            Socket s = obj as Socket;
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = new byte[4096];

            while (connected)
            {                
                try
                {
                    int result = s.Receive(buffer);

                    if (result > 0)
                    {
                        Console.Write(encoder.GetString(buffer, 0, result));
                    }
                }
                catch (System.Exception)
                {
                    connected = false;
                }
            }
        }

        static void Main(string[] args)
        {
            Socket s = null;

            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8222);

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] incommingBuffer = new byte[4096];

            bool bQuit = false;

            while (bQuit == false)
            {
                while (connected == false)
                {
                    try
                    {
                        s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        s.Connect(ipLocal);
                        connected = true;

                        var myThread = new Thread(clientReceiveThread);
                        myThread.Start(s);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No server active");
                        Thread.Sleep(1000);
                    }
                }

                Thread.Sleep(1000);
                
                while (connected == true)
                {
                    Thread.Sleep(100);

                    Console.Write(">>");
                    String Msg = Console.ReadLine();

                    byte[] outputBuffer = encoder.GetBytes(Msg);

                    try
                    {
                        int bytesSent = s.Send(outputBuffer);
                    }
                    catch (System.Exception)
                    {
                        connected = false;
                    }
                }

                s = null;
            }
        }
    }
}
