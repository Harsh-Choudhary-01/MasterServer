using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace MasterServerGame
{
     class ServerGet
    {
        static List<string> ipAddresses = new List<string>();
        static byte[] buffer = new byte[1024];
        static List<Socket> clientSockets = new List<Socket>();
        static Socket sListener;
        static AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
        public void Initalize()
        {
            Console.Write("Starting");
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 4000);
            sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sListener.Bind(ipEndPoint);
            sListener.Listen(5);
            sListener.BeginAccept(aCallback, sListener);
        }
        static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket = sListener.EndAccept(AR);
            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
            sListener.BeginAccept(aCallback, sListener);

        }
        static void RecieveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int recieved = socket.EndReceive(AR);
            byte[] tempBuffer = new byte[recieved];
            Array.Copy(buffer, tempBuffer, recieved);
            string text = Encoding.ASCII.GetString(tempBuffer);
            if(text == "get")
            {
                int remaining = ipAddresses.Count;
                foreach (string IPAddresses in ipAddresses)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(IPAddresses);
                    remaining--;    
                    socket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(SendCallback), Tuple.Create(socket , remaining));
                }
            }
            Console.WriteLine("Text Recieved: " + text);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
        }
        static void SendCallback(IAsyncResult IA)
        {
            Tuple<Socket, int> state = (Tuple < Socket, int>)IA.AsyncState;
            if (state.Item2 <= 0)
            {
                Socket socket = (Socket)IA.AsyncState;
                socket.EndSend(IA);
            }
        }
    }
}
