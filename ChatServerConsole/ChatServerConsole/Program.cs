﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace ChatServerConsole {
    class Program {

        static TcpListener server = null;

        static IPAddress ip = IPAddress.Parse("127.0.0.1");
        static int port = 13000;
        List<TcpClient> clients = new List<TcpClient>();
        Byte[] bytes = new Byte[256];
        String data = null;

        public async Task tcpServer() {
            CancellationTokenSource cts = new CancellationTokenSource();

            try {
                server = new TcpListener(ip, port);
                server.Start();
                Console.Write("1");

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
                clients.Add(client);
                
                await messages(client, ct);
                
            }
        }

        private async Task messages(TcpClient client, CancellationToken ct) {
           

            NetworkStream stream = client.GetStream();

            byte[] inBuffer = new byte[256];

            List<byte> buffer = new List<byte>();
           
           
            /* while ((i=stream.Read(inBuffer, 0, inBuffer.Length)) != 0) {

                 buffer.AddRange(inBuffer);
                 Console.Write("6");

             }*/
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

           /* foreach (TcpClient c in clients) {
               stream = c.GetStream();
               stream.Write(ccm.Serialize(), 0, ccm.Serialize().Length);
                
           }*/

        }

        public void udpServer() {
            

            UdpClient listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);

            try {
                while (true) {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);

                    ChatClientMessage ccm = ChatClientMessage.Deserialize(bytes);

                    string message = ccm.UserName + " says: " + ccm.Message;
                    ChatServerMessage csm = new ChatServerMessage(message);

                    listener.Send(csm.Serialize(), csm.Serialize().Length);
                }

            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            finally {
                listener.Close();
            }
        }







        static void Main(string[] args) {
            Program x = new Program();
            Task.WaitAll(x.tcpServer());
           // x.udpServer();
            
            
            

            





            //IPAddress ip = IPAddress.Parse("233");
            // TcpListener server = new TcpListener(ip,3333);

            // server.Start();

            // TcpClient client = server.AcceptTcpClient();
            //
            //  NetworkStream stream = client.GetStream();

            // stream.Read()



        }
    }
}
