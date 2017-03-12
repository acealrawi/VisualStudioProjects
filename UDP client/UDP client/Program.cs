using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDP_client {
    class Program {
        static void Main(string[] args) {
            UdpClient udpClient = new UdpClient();
            try {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 13000);
                udpClient.Connect("127.0.0.1", 13000);

                // Sends a message to the host to which you have connected.
                ChatClientMessage ccm = new ChatClientMessage("Ali", "Hi Dion");
                udpClient.Send(ccm.Serialize(), ccm.Serialize().Length);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref ip);

                ChatServerMessage csm = ChatServerMessage.Deserialize(receiveBytes);
                

                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received " + csm.Message);
                Console.WriteLine("This message was sent from " + ip.Address.ToString() + " on their port number " + ip.Port.ToString());

                
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
