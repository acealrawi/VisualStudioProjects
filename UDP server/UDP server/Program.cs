using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDP_server {
    class Program {
        static void Main(string[] args) {

            
            try {
                UdpClient udpClient = new UdpClient(13000);
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 13000);
                

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                while (true) {
                    Byte[] receiveBytes = udpClient.Receive(ref ip);

                    ChatClientMessage ccm = ChatClientMessage.Deserialize(receiveBytes);


                    // Uses the IPEndPoint object to determine which of these two hosts responded.
                    Console.WriteLine("This is the message you received " + ccm.Message + " from user: " + ccm.UserName);
                    Console.WriteLine("This message was sent from " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());
                }
                // Sends a message to the host to which you have connected.
               // string replay = ccm.UserName + "says: " + ccm.Message;
               // ChatServerMessage csm = new ChatServerMessage(replay.ToUpper());

               /* udpClient.Connect(RemoteIpEndPoint.Address.ToString(),RemoteIpEndPoint.Port);

                udpClient.Send(csm.Serialize(), csm.Serialize().Length);*/

                

                


                

                //udpClient.Close();
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
