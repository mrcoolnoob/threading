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
		// initially set to the non-signaled-state => false
		ManualResetEvent mrse = new ManualResetEvent(false);

		public Socket()
		{
			udpClient = new UdpClient(1000); // localport
			udpClient.Connect(endPoint); // remoteport
			udpClient.Client.ReceiveTimeout = 100;
		}

		public void StartServer(Thread receiver)
		{
			// in order for mrse (ManualResetEvent) you need to pass it as an argument
			//receiver.Start(mrse);
			mrse.Reset();
            receiver.Start();
        }

        public void SendPacket()
		{
			try
			{
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

		public void ShutDownServer()
		{
            // Set the stopListeningEvent to signal the ListeningServer() method to exit gracefully
            mrse.Set();
        }
		public void ReceivePacket()
		{
			byte[] msg = udpClient.Receive(ref endPoint);
			Console.WriteLine(Encoding.ASCII.GetString(msg));
		}

		public void ListeningServer(object state)
		{
            Console.WriteLine("Server starting");
            //ManualResetEvent stopListeningEvent = (ManualResetEvent)state;
            while (true)
			{
				try
				{
                    byte[] msg = udpClient.Receive(ref endPoint);
                    Console.WriteLine(Encoding.ASCII.GetString(msg));

                }
				catch (Exception ex)
				{

                }
                // wait zero second for the signal if signaled it returns True
                // True then simply exit the Thread
                if (mrse.WaitOne(0))
                {
                    Console.WriteLine("Signal received to end the thread");
                    break;
                }
            }
            Console.WriteLine("Server exiting");
        }
    }
}

