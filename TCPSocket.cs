using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Threading;



namespace Microsoft.Samples.Kinect.BodyBasics
{
    public static class TCPSocket
    {

        private static TcpClient client;
        private static NetworkStream stream;

        private static BlockingCollection<byte[]> m_Queue = new BlockingCollection<byte[]>();

        static TCPSocket()
        {

            String[] args = App.Args;

            try
            {                
                client = new TcpClient(args[0], Convert.ToInt16(args[1]));
                stream = client.GetStream();
            }
            catch(Exception e)
            {
                client = new TcpClient("127.0.0.1", 4444); //default connect to localhost
                stream = client.GetStream();
                //Console.WriteLine("exception: " + e.Message);
            }

            var thread = new Thread(
            () =>
            {
                //while (true) Console.WriteLine(m_Queue.Take());
                while (true) {
                    var tmp = m_Queue.Take();
                    stream.Write(tmp, 0, tmp.Length);
                }
            });
            thread.IsBackground = true;
            thread.Start();

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
