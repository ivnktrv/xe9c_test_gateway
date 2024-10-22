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
        Console.WriteLine("##### СПИСОК IP #####\n");
        for (int i = 0; i < ips.Length; i++)
        {
            Console.WriteLine($"[{i}] {ips[i]}");
        }
        
        Xe9c_gateway x = new(ips[1], 5050);
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
