using System.Threading;
using ChatClientA;

public class Program
{
    private static void Main(string[] args)
    {
        Socket socket = new Socket();
        Thread sender = new Thread(socket.SendPacket);
        Thread receiver = new Thread(socket.ListeningServer);

        receiver.Start();

        while (true)
        {
            Console.WriteLine("Enter packet #: ");
            var num = int.Parse(Console.ReadLine());
            switch (num)
            {
                case 1:
                    if (receiver.IsAlive) {
                        socket.Listen = false;
                        Console.WriteLine("Waiting for receiver thread to finish");
                        receiver.Join();
                    }
                    Console.WriteLine("Sender started");
                    sender = new Thread(socket.SendPacket);
                    sender.Start();
                    Console.WriteLine("Waiting for sender to complete");
                    sender.Join();
                    Console.WriteLine("Sender thread execution completed");
                    socket.Listen = true;
                    receiver = new Thread(socket.ListeningServer);
                    receiver.Start();
                    Console.WriteLine("Receiver started");
                    break;
                case 2:

                    break;
                default:
                    break;
            }
            if (num == 9) break;
        }

    }
}