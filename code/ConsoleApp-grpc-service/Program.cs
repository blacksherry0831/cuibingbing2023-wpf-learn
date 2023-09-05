using Nerdbank.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcGreeter;

namespace ConsoleApp_grpc_service
{


    class Program { 
    
    private static async Task RunAsync()
    {
        var server = new Grpc.Core.Server
        {
            Ports = { { "127.0.0.1", 20000, ServerCredentials.Insecure } },
           // Services = { GreeterService. }
            
        };

            var svr = Greeter.BindService(new GreeterService());

            server.Services.Add(svr);

        server.Start();
        Console.WriteLine($"Server started under [127.0.0.1:5000]. Press Enter to stop it...");
        Console.ReadLine();
        await server.ShutdownAsync();
    }

    static async Task Main(string[] args)
    {
            await RunAsync();
    }
    }


    


}
