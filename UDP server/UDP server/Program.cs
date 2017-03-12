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
            UdpClient udpClient = new UdpClient(13000);            
            try {
                
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 13000);
                

                while (true) {
                    Byte[] receiveBytes = udpClient.Receive(ref ip);
                    ChatClientMessage ccm = ChatClientMessage.Deserialize(receiveBytes);
                    Console.WriteLine("This is the message you received " + ccm.Message + " from user: " + ccm.UserName);
                    ChatServerMessage csm = new ChatServerMessage(ccm.Message.ToUpper());
                    Console.WriteLine("This message was sent from " + ip.Address.ToString() + " on their port number " + ip.Port.ToString());
                    udpClient.Connect(ip);
                    udpClient.Send(csm.Serialize(), csm.Serialize().Length);
                }

            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
