using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace ChatServerConsole {
    class Server {

        static TcpListener server = null;

        static IPAddress ip = IPAddress.Parse("127.0.0.1");
        static int port = 13000;
        
        Byte[] bytes = new Byte[256];
        String data = null;

        public async Task tcpServer() {
            CancellationTokenSource cts = new CancellationTokenSource();

            try {
                server = new TcpListener(ip, port);
                server.Start();


                await acceptClient(server, cts.Token);
            }
            catch (SocketException e) {

                Debug.WriteLine(e);
            }
            finally {
                cts.Cancel();
                server.Stop();
            }
        }


        private async Task acceptClient(TcpListener listener, CancellationToken ct) {
            
            while (!ct.IsCancellationRequested) {
                TcpClient client = await server.AcceptTcpClientAsync();
                
                
                await messages(client, ct);
                
            }
        }

        private async Task messages(TcpClient client, CancellationToken ct) {
           

            NetworkStream stream = client.GetStream();

            byte[] inBuffer = new byte[256];

            List<byte> buffer = new List<byte>();
            do {

                stream.Read(inBuffer, 0, inBuffer.Length);
                buffer.AddRange(inBuffer);
                Console.WriteLine("Received: ");
            }
            while (stream.DataAvailable);
            
            
            ChatClientMessage ccm = ChatClientMessage.Deserialize(buffer.ToArray());
           
            string message = ccm.UserName + " says " + ccm.Message;
            Console.WriteLine(message);
            
           ChatServerMessage csm = new ChatServerMessage(message);
           stream.Write(csm.Serialize(), 0, csm.Serialize().Length);

        }

        public void udpServer() {
            UdpClient udpClient = null;
            try
            {
                udpClient = new UdpClient(13000);
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 13000);


                while (true)
                {
                    Byte[] receiveBytes = udpClient.Receive(ref ip);
                    ChatClientMessage ccm = ChatClientMessage.Deserialize(receiveBytes);
                    Console.WriteLine("This is the message you received:   " + ccm.Message + "   from user: " + ccm.UserName);
                    ChatServerMessage csm = new ChatServerMessage(ccm.Message.ToUpper());
                    Console.WriteLine("This message was sent from " + ip.Address.ToString() + " on their port number " + ip.Port.ToString());
                    udpClient.Connect(ip);
                    udpClient.Send(csm.Serialize(), csm.Serialize().Length);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally{
                udpClient.Close();
            }


        }







        static void Main(string[] args) {
            Server server = new Server();
            bool tcp = true;
            if (tcp){
                Task.WaitAll(server.tcpServer());
            }
            else
            {
                server.udpServer();
            }
        }
    }
}
