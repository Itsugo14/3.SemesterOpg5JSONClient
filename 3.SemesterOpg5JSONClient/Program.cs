using System;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

class Client
{
    static void Main()
    {
        Console.WriteLine("TCP Client:");

        using TcpClient client = new TcpClient("127.0.0.1", 8000);
        using NetworkStream ns = client.GetStream();
        using StreamReader reader = new StreamReader(ns);
        using StreamWriter writer = new StreamWriter(ns) { AutoFlush = true };

        while (true)
        {
            Console.WriteLine("Choose method (Random, Add, Subtract) or type 'exit' to quit:");
            string method = Console.ReadLine();
            if (method.ToLower() == "exit") break;

            Console.WriteLine("Enter number 1:");
            int number1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter number 2:");
            int number2 = int.Parse(Console.ReadLine());

            var request = new Request { Method = method, Number1 = number1, Number2 = number2 };
            string jsonRequest = JsonSerializer.Serialize(request);

            writer.WriteLine(jsonRequest);
            string jsonResponse = reader.ReadLine();
            var response = JsonSerializer.Deserialize<Response>(jsonResponse);

            if (response.Error != null)
                Console.WriteLine($"Error: {response.Error}");
            else
                Console.WriteLine($"Result: {response.Result} - {response.Message}");
        }
    }
}

public class Request
{
    public string Method { get; set; }
    public int Number1 { get; set; }
    public int Number2 { get; set; }
}

public class Response
{
    public int? Result { get; set; }
    public string Message { get; set; }
    public string Error { get; set; }
}
