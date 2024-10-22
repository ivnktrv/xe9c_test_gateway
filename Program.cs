﻿using System.Net.Sockets;

namespace xe9c_gateway;

internal class Program
{
    static void Main(string[] args)
    {
        Xe9c_gateway x = new(args[0], int.Parse(args[1]));
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
