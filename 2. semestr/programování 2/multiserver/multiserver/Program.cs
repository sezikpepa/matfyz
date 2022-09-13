using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MultiServer
{


    public class PlayingPair
    {
        private Socket? player1;
        private Socket? player2;

        public TimeSpan player1Status;
        public TimeSpan player2Status;

        public PlayingPair()
        {
            player1 = null;
            player2 = null;

            this.player1Status = DateTime.Now.TimeOfDay;
            this.player2Status = DateTime.Now.TimeOfDay;
        }

        public bool IsFull()
        {
            return player1 != null && player2 != null;
        }

        public void PlayerInput(Socket player)
        {
            if (this.player1 == null)
            {
                this.player1 = player;
                byte[] data = Encoding.ASCII.GetBytes("white");
                this.player1.Send(data);
                return;
            }
            if (this.player2 == null)
            {
                this.player2 = player;
                byte[] data = Encoding.ASCII.GetBytes("black");
                this.player2.Send(data);
            }
        }

        public void checkPlayersConnected()
        {

        }

        private bool checkPlayer1Connected()
        {
            return true;
        }
    }


    class Program
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 100;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        private static PlayingPair[] playingPairs = new PlayingPair[5];
       

        static void Main()
        {
            Console.Title = "MatfyzChessServer";
            SetupServer();
            //TimeSpan timeStart = DateTime.Now.TimeOfDay;
            /*
            while (true)
            {
                if(DateTime.Now.TimeOfDay.TotalSeconds - timeStart.TotalSeconds > 5)
                {
                    
                }
            }
            */
            Console.ReadLine(); // When we press enter close everything
            CloseAllSockets();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("Server setup complete");
        }

        /// <summary>
        /// Close all connected client (we do not need to shutdown the server socket as its connections
        /// are already closed with the clients).
        /// </summary>
        private static void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            serverSocket.Close();
        }

        private static void FindFreePair(Socket socket)
        {
            foreach(var element in playingPairs)
            {
                if (element.IsFull() == false)
                {
                    element.PlayerInput(socket);
                }
            }
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            clientSockets.Add(socket);
            
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            Console.WriteLine("Client connected, waiting for request...");
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private static bool checkForSpecialMessages(string message)
        {
            return false;
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Received Text: " + text);

            if (checkForSpecialMessages(text))
            {

            }

            foreach(var element in playingPairs)
            {
                if(element.IsFull() == true)
                {

                }
            }
        
            else if (playingPair[1] != null)
            {
                if (current == playingPair[0])
                {
                    byte[] data = Encoding.ASCII.GetBytes(text);
                    playingPair[1].Send(data);
                    Console.WriteLine("Message sent to client");
                }
                else
                {
                    byte[] data = Encoding.ASCII.GetBytes(text);
                    playingPair[0].Send(data);
                    Console.WriteLine("Message sent to client");
                }           
            }

            

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }
    }
}