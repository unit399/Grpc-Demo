using Calculator;
using Greet;
using Grpc.Core;
using server;

const int Port = 50051;

Server server = null;

try
{
    server = new Server()
    {
        Services = 
        { 
            GreetingService.BindService(new GreetingServiceImpl()),
            CalculatorService.BindService(new CalculatorServiceImpl())
        },
        Ports =
        {
            new ServerPort("localhost", Port, ServerCredentials.Insecure)
        }       
    }; 

    server.Start();
    Console.WriteLine("The server is listening on the port : " + Port);
    Console.ReadKey();
}
catch (IOException ex)
{
    Console.WriteLine("The server failed to start : " + ex.Message);
}
finally
{
    if (server != null)
    {
        server.ShutdownAsync().Wait();
    }
}
