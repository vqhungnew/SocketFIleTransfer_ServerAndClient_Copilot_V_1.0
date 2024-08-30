using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

class FileTransfer
{
    static void Main()
    {
        //Call Server Function
        fileTransferServer();

        //Call Client Function
        fileTransferClient();
    }
    
    //Build Server for FILE Transfer
    private static void fileTransferServer()
    {
        // Set up the server to listen on port 11000
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11000);
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        listener.Bind(endPoint);
        listener.Listen(10);

        Console.WriteLine("Waiting for a connection...");
        Socket handler = listener.Accept();
        Console.WriteLine("Client connected.");

        // Specify the file to be sent
        string filePath = "C:\\test.txt";
        byte[] fileData = File.ReadAllBytes(filePath);

        // Send the file
        handler.Send(fileData);
        Console.WriteLine("File sent.");

        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }

    //Build Client for FILE Transfer
    private static void fileTransferClient()
    {
        /// Set up the client to connect to the server
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
        Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        sender.Connect(endPoint);
        Console.WriteLine("Connected to server.");

        // Receive the file
        byte[] buffer = new byte[1024];
        int bytesRead;
        using (FileStream fs = new FileStream("received.txt", FileMode.Create))
        {
            while ((bytesRead = sender.Receive(buffer)) > 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }

        Console.WriteLine("File received.");

        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }
}