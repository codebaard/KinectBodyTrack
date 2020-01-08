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


namespace Microsoft.Samples.Kinect.BodyBasics
{
    public static class TCPSocket
    {

        private static TcpClient client;
        private static NetworkStream stream;

        private static BlockingCollection<byte[]> m_Queue = new BlockingCollection<byte[]>();

        private static string remoteHost = null;
        private static Int16 remotePort = 0;

        private static int TIMEOUT = 1000;

        static TCPSocket()
        {

            String[] args = App.Args;

            remoteHost = args[0];
            remotePort = Convert.ToInt16(args[1]);

            connectToServer();

            var thread = new Thread(
            () =>
            {
                while (true) {
                    var tmp = m_Queue.Take();
                    try
                    {
                        stream.Write(tmp, 0, tmp.Length);
                        stream.Flush();
                        Thread.Sleep(1); //prevent NIC from flooding remote application
                    }
                    catch (IOException e)
                    {
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
            bool connected = false;
            stream = null;

            try
            {
                client = new TcpClient(remoteHost, remotePort);
                stream = client.GetStream();

                connected = true;
            }
            catch (Exception e)
            {
                //client = new TcpClient("127.0.0.1", 4444); //default connect to localhost
                //stream = client.GetStream();
                //Console.WriteLine("exception: " + e.Message);
                connected = false;
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
}
