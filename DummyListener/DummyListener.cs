using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.IO;
using System.Net;

namespace DummyListener
{
    class DummyListener
    {
        
        public static void Main(string[] args)
        {
            TcpListener server = null;

            try
            {
                // Set the TcpListener on port 4444.
                Int32 port = 4444;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[1400];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;
                    bool ConnectionLost = true;

                    // Loop to receive all the data sent by the client.
                    do
                    {
                        try
                        {
                            i = stream.Read(bytes, 0, bytes.Length);
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            Console.Write(data);
                            ConnectionLost = false;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            ConnectionLost = true;
                        }
                    } while (ConnectionLost == false);

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

        }
    }

}
