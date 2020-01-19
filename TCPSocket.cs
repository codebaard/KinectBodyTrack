
#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Windows.Forms;

using System.Diagnostics;




namespace Microsoft.Samples.Kinect.BodyBasics
{
    public static class TCPSocket
    {
        //this class deals with all the network traffic.
        //it works with a threadsafe blocking collection to enhance asychronous behaviour


        private static TcpClient client;
        private static NetworkStream stream;

        private static BlockingCollection<byte[]> m_Queue = new BlockingCollection<byte[]>();

        private static string remoteHost = null;
        private static Int16 remotePort = 0;

        private const int TIMEOUT = 1000;
        private const string LOCALHOST = "127.0.0.1";
        private const int LOCALPORT = 3000;

        static TCPSocket()
        {

            try
            {
                String[] args = App.Args;
                remoteHost = args[0];
                remotePort = Convert.ToInt16(args[1]);
            }
            catch
            {
                //no cli parameters provided - using localhost
                //WARNING: Listener must be available! -> autolaunch project "DummyListener"
                remoteHost = LOCALHOST;
                remotePort = LOCALPORT;

                DummyListenerHandler.startApplication();
            }

            connectToServer();

            var thread = new Thread(
            () =>
            {
                while (true) {
                    var tmp = m_Queue.Take(); //take next element - strictly FIFO
                    try
                    {
                        stream.Write(tmp, 0, tmp.Length);
                        stream.Flush();
                        Thread.Sleep(1); //prevent NIC from flooding remote application
                    }
                    catch (IOException e)
                    {
                        //connection lost -> trying to reconnect after user acknowledged
                        MessageBox.Show(e.Message + " - Attempting to reconnect...", "Network Error");
                        Thread.Sleep(TIMEOUT);
                        connectToServer();
                    }

                }
            });
            thread.IsBackground = true;
            thread.Start();

        }

        static void connectToServer()
        {
            stream = null;

            try
            {
                client = new TcpClient(remoteHost, remotePort);
                stream = client.GetStream();
            }
            catch (Exception e)
            {
                //this happens only, when the remotehost is localhost and the listener is closed by the user deliberately
                //it will terminate the programm to ensure nothing unforeseeable happens since we cannot assume the right state of every component
#if (DEBUG)
                Console.WriteLine("exception: " + e.Message);
#endif
                client = null;
                stream = null;
            }
        }

        public static void sendmsg(String msg)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

            // Send the message to the connected TcpServer. 
            m_Queue.Add(data);

        }

    }

    public static class DummyListenerHandler{

        //path is supposed to be constant since its part of the visual studio solution"
        private const string path = ".\\..\\..\\..\\DummyListener\\bin\\Release\\DummyListener.exe";

        public static void startApplication()
        {
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.FileName = path;
                    myProcess.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

    }
}
