using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer1 {
    public static class Program {
        static TcpListener server = null;
        static IPAddress ip = IPAddress.Parse("");
        static int port;
        static List<TcpClient> clients = new List<TcpClient>();

        public static void reciveSend() {
            byte[] dataBytes = new byte[sizeof(int)];
            string data;

            while (true) {
                TcpClient client = server.AcceptTcpClientAsync();

                clients.Add(client);

                NetworkStream stream = client.GetStream();

                int i;


                while ((i = stream.Read(dataBytes, 0, dataBytes.Length)) != 0) {

                    data = Encoding.Unicode.GetString(dataBytes, 0, i);

                }

                foreach (TcpClient c in clients) {
                    string newData = "";

                    byte[] msg = Encoding.Unicode.GetBytes(newData);


                    stream.Write(msg, 0, msg.Length);
                }


            }
        }

        public static void listener() {
            try {
                server = new TcpListener(ip, port);
                server.Start();
                reciveSend();

            }
            catch (SocketException e) {

            }

            finally {
                server.Stop();
            }
        }

        public static void Main(string[] args) {
            listener();
        }
    }
}
