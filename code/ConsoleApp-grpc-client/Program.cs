using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrpcGreeter;


namespace ConsoleApp_grpc_client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RunAsync();

            Console.ReadKey();

        }
        private static async Task RunAsync()
        {

            Channel channel = new Channel("127.0.0.1:20000", ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);
            String user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("Greeting: " + reply.Message);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();


          
          


            await channel.ShutdownAsync();
        }
    }
}
