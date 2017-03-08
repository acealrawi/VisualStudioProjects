using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ChatServerUWP {
    class ChatServer {

        TcpListener server = null;

        IPAddress ip = IPAddress.Parse("127.0.0.1");
        int port = 13000;
        List<TcpClient> tcpClients = new List<TcpClient>();
        List<IPEndPoint> udpClients = new List<IPEndPoint>();
        Byte[] bytes = new Byte[256];
        

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
                
                tcpClients.Add(client);
                
                await messages(client, ct);

            }
        }

        private async Task messages(TcpClient client, CancellationToken ct) {
            

            NetworkStream stream = client.GetStream();

            byte[] inBuffer = new byte[256];

            List<byte> buffer = new List<byte>();
           

            /* while ((i=stream.Read(inBuffer, 0, inBuffer.Length)) != 0) {

                 buffer.AddRange(inBuffer);
                 

             }*/
            do {

                stream.Read(inBuffer, 0, inBuffer.Length);
                buffer.AddRange(inBuffer);
                
            }
            while (stream.DataAvailable);

           
            ChatClientMessage ccm = ChatClientMessage.Deserialize(buffer.ToArray());
            
            string message = ccm.UserName + " says: " + ccm.Message;
            
            ChatServerMessage csm = new ChatServerMessage(message);

            foreach (TcpClient c in tcpClients) {
            stream = c.GetStream();
            stream.Write(ccm.Serialize(), 0, ccm.Serialize().Length);

            }

        }

        public async Task udpServer() {


            UdpClient listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);

            try {
                while (true) {
                    
                    UdpReceiveResult result = await listener.ReceiveAsync();
                    udpClients.Add(groupEP);
                    
                    ChatClientMessage ccm = ChatClientMessage.Deserialize(bytes);

                    string message = ccm.UserName + " says: " + ccm.Message;
                    ChatServerMessage csm = new ChatServerMessage(message);

                    foreach (IPEndPoint c in udpClients) {

                        await listener.SendAsync(csm.Serialize(), csm.Serialize().Length, c);
                    }
                }

            }
            catch (Exception e) {
                Debug.WriteLine(e);
            }
            
        }
    }
}
