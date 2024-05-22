using System.Net.Sockets;
using System.Net;
using System.Text;

namespace TCPLargeMessageTest
{
    public sealed class Server
    {
        public Server(int port)
        {
            Port = port;
        }

        public int Port { get; }

        public void Start()
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            TcpListener listener = new(ipEndPoint);

            try
            {
                listener.Start();

                using TcpClient handler = listener.AcceptTcpClient();
                using NetworkStream stream = handler.GetStream();

                byte[] buffer = new byte[Program.MessageSize];
                int received = stream.Read(buffer);

                var message = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine($"Message received: \"{message.Length}\"");
            }
            finally
            {
                listener.Stop();
            }
        }
    }
    public sealed class Client
    {
        #region Lifetime
        public Client(IPEndPoint ipEndPoint)
        {
            IpEndPoint = ipEndPoint;
        }
        public IPEndPoint IpEndPoint { get; }
        public TcpClient TcpClient { get; private set; }

        public void Connect()
        {
            TcpClient = new();
            TcpClient.Connect(IpEndPoint);
        }
        #endregion

        #region Messaging
        public void SendMessage(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            SendData(messageBytes);
        }
        public void SendData(byte[] bytes)
        {
            using NetworkStream stream = TcpClient.GetStream();
            stream.Write(bytes);
        }
        #endregion
    }

    internal class Program
    {
        public static int MessageSize = (int)(1.5 * 1024 * 1024 * 1024 / 2);  // Remark: 1.5/2Gb; Must fit within the positive range of an int

        #region Entry
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Loopback;
            short port = 7999;
            var ipEndPoint = new IPEndPoint(ipAddress, port);

            // Start server
            new Thread(() =>
            {
                new Server(port).Start();
            }).Start();

            // Start a single client
            Client client = new(ipEndPoint);
            client.Connect();
            client.SendMessage(GenerateSampleMessage());

            Console.WriteLine("Client is done.");
        }
        #endregion

        #region Helpers
        public static string GenerateSampleMessage()
        {
            var builder = new StringBuilder(MessageSize, MessageSize);
            for (int i = 0; i < MessageSize; i++)
                builder.Append('!');

            return builder.ToString();
        }
        #endregion
    }
}
