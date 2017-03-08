using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;

namespace msclient {
    class Program {

        static void Connect(String server, String message) {
            try {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);

               

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();
                ChatClientMessage ccm = new ChatClientMessage("x", message);

                // Send the message to the connected TcpServer. 
                stream.Write(ccm.Serialize(), 0, ccm.Serialize().Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                byte [] data = new Byte[256];
                List<byte> buffer = new List<byte>();

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                do {

                    stream.Read(data, 0, data.Length);
                    buffer.AddRange(data);

                }
                while (stream.DataAvailable);
                
                ChatServerMessage csm = ChatServerMessage.Deserialize(buffer.ToArray());
                
                Console.WriteLine("Received: "+ csm.Message);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e) {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e) {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }

        public static void udpClient() {
            // This constructor arbitrarily assigns the local port number.
            UdpClient udpClient = new UdpClient(13000);
            try {
                udpClient.Connect("127.0.0.1", 13000);

                // Sends a message to the host to which you have connected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

                udpClient.Send(sendBytes, sendBytes.Length);

                

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.Unicode.GetString(receiveBytes);

                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received " +
                                             returnData.ToString());
                Console.WriteLine("This message was sent from " +
                                            RemoteIpEndPoint.Address.ToString() +
                                            " on their port number " +
                                            RemoteIpEndPoint.Port.ToString());

                udpClient.Close();
                

            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
        static void Main(string[] args) {
            Connect("127.0.0.1", "plz plz work");
           // udpClient();

        }
    }
}
