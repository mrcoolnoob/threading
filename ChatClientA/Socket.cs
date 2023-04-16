using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClientA
{
	public class Socket
	{
		UdpClient udpClient;
		IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 2000);
        private readonly object listenLock = new object();
        private bool listen = true;

        public bool Listen
        {
            get
            {
                lock (listenLock)
                {
                    return listen;
                }
            }
            set
            {
                lock (listenLock)
                {
                    listen = value;
                }
            }
        }

        public Socket()
		{
			udpClient = new UdpClient(1000); // localport
			udpClient.Connect(endPoint); // remoteport
            udpClient.Client.ReceiveTimeout = 100;
        }

        public void SendPacket()
		{
			try
			{
                udpClient.Client.ReceiveTimeout = 10000;
                byte[] msg = Encoding.UTF8.GetBytes("Hey");
				udpClient.Send(msg);
				byte[] ack = udpClient.Receive(ref endPoint);
                Console.WriteLine("Acknowledgement received");
                Console.WriteLine(Encoding.ASCII.GetString(ack));
			} catch (Exception e)
			{
				Console.WriteLine("Acknowledgement not receive");
			}

        }

		public void ReceivePacket()
		{
			byte[] msg = udpClient.Receive(ref endPoint);
			Console.WriteLine(Encoding.ASCII.GetString(msg));
		}

		public void ListeningServer()
		{
			while(Listen)
			{
				try
				{
                    byte[] msg = udpClient.Receive(ref endPoint);
                    Console.WriteLine(Encoding.ASCII.GetString(msg));
                    Console.WriteLine("listening");
                }
				catch (Exception ex)
				{
					continue;
				}

            }
			Console.WriteLine("Receiver thread last line");
		}
	}
}

