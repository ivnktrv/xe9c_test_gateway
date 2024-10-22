using System.Net.Sockets;
using System.Net;

namespace xe9c_gateway;

internal class Program
{
    static void Main(string[] args)
    {
        List<string> getIPs = new List<string>();
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                getIPs.Add(ip.ToString());
            }
        }
        string[] ips = getIPs.ToArray();
        Xe9c_gateway x = new(ips[0], 5050);
        Console.WriteLine(x.GatewayInfo());
        Socket s = x.CreateGateway();

        while (true)
        {
            Socket clientSocket = s.Accept();
            Console.WriteLine("Connected client: "+clientSocket.RemoteEndPoint);
            x.AddClient(clientSocket);
            Task.Run(() => { x.HandleClient(clientSocket); });
        }
        
    }
}
