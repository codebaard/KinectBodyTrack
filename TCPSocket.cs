using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;



namespace Microsoft.Samples.Kinect.BodyBasics
{
    class TCPSocket
    {

        private TcpClient client;
        private NetworkStream stream;

        public TCPSocket(string host, int port)
        {
            try
            {                
                client = new TcpClient(host, port);
                stream = client.GetStream();
            }
            catch(Exception e)
            {
                Console.WriteLine("exception: " + e.Message);
            }            

        }
        public TCPSocket()
        {
            try
            {                
                client = new TcpClient("127.0.0.1", 4444);
                stream = client.GetStream();
            }
            catch(Exception e)
            {
                Console.WriteLine("exception: " + e.Message);
            }            

        }

        ~TCPSocket()
        {
            // Close everything.
            stream.Close();
            client.Close();
        }
        
        //void Connect()
        //{
        //    try
        //    {


        //        Console.WriteLine("Sent: {0}", message);

        //        // Receive the TcpServer.response.

        //        // Buffer to store the response bytes.
        //        data = new Byte[256];

        //        // String to store the response ASCII representation.
        //        String responseData = String.Empty;

        //        // Read the first batch of the TcpServer response bytes.
        //        Int32 bytes = stream.Read(data, 0, data.Length);
        //        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        //        Console.WriteLine("Received: {0}", responseData);


        //    }
        //    catch (ArgumentNullException e)
        //    {
        //        Console.WriteLine("ArgumentNullException: {0}", e);
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("SocketException: {0}", e);
        //    }
        //}

        public void sendmsg(String msg)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
         
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
        }

    }
}
