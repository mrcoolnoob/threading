using System.Threading;
using ChatClientA;

public class Program
{
    private static void Main(string[] args)
    {
        Socket socket = new Socket();
        Thread sender = new Thread(socket.SendPacket);
        //Thread receiver = new Thread(new ParameterizedThreadStart(socket.ListeningServer));
        Thread receiver = new Thread(socket.ListeningServer);

        socket.StartServer(receiver);

        while (true)
        {
            Console.WriteLine("Enter packet #: ");
            var num = int.Parse(Console.ReadLine());
            switch (num)
            {
                case 1:
                    if (receiver.IsAlive)
                    { 
                        Console.WriteLine("Shutting down receiver");
                        socket.ShutDownServer();
                        Console.WriteLine("Waiting for receiver to completely shutdown");
                        receiver.Join();
                    }
                    Console.WriteLine("Sender started");
                    sender = new Thread(socket.SendPacket);
                    sender.Start();
                    Console.WriteLine("Waiting for sender to complete");
                    sender.Join();
                    Console.WriteLine("Sender thread execution completed");
                    receiver = new Thread(socket.ListeningServer);
                    socket.StartServer(receiver);
                    Console.WriteLine("Receiver started");
                    break;
                default:
                    break;
            }
            if (num == 9) break;
        }

    }
}