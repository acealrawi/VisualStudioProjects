using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;

namespace msclient {
    class Program {

        static void connectTCP(string username, string message) {
            try {
                
                int port = 13000;
                String server = "127.0.0.1";
                TcpClient client = new TcpClient(server, port);

                NetworkStream stream = client.GetStream();
                ChatClientMessage ccm = new ChatClientMessage(username, message);

                stream.Write(ccm.Serialize(), 0, ccm.Serialize().Length);

                Console.WriteLine("Sent: {0}", message);

                byte [] data = new Byte[256];
                List<byte> buffer = new List<byte>();

 
                String responseData = String.Empty;


                do {

                    stream.Read(data, 0, data.Length);
                    buffer.AddRange(data);

                }
                while (stream.DataAvailable);
                
                ChatServerMessage csm = ChatServerMessage.Deserialize(buffer.ToArray());
                
                Console.WriteLine("Received: "+ csm.Message);

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
        public static void connectUDP(string username, string msg)
        {
            UdpClient udpClient = null;
            try
            {
                udpClient = new UdpClient();
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 13000);
                udpClient.Connect("127.0.0.1", 13000);

                
                ChatClientMessage ccm = new ChatClientMessage(username, msg);
                udpClient.Send(ccm.Serialize(), ccm.Serialize().Length);

                
                Byte[] receiveBytes = udpClient.Receive(ref ip);

                ChatServerMessage csm = ChatServerMessage.Deserialize(receiveBytes);


               
                Console.WriteLine("This is the message you received:   " + csm.Message);
                Console.WriteLine("This message was sent from " + ip.Address.ToString() + " on their port number " + ip.Port.ToString());


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                udpClient.Close();
            }

        }

       
        static void Main(string[] args) {
            bool tcp = true;

            if (tcp){
                connectTCP("ali", "lol");
            }
            else{
                connectUDP("ali", "hi Dion");
            }
            
            
           

        }
    }
}
