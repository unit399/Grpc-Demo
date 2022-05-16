using Calculator;
using client;
using Dummy;
using Greet;
using Grpc.Core;

const string target = "127.0.0.1:50051";

Channel channel = new Channel(target, ChannelCredentials.Insecure);

await channel.ConnectAsync().ContinueWith((task) =>
{
    if (task.Status == TaskStatus.RanToCompletion)
        Console.WriteLine("The client connected successfully");
});

var greetClient = new GreetingService.GreetingServiceClient(channel);

StaticClients.DoSimpleGreet(greetClient);
await StaticClients.DoGreetEveryone(greetClient);
await StaticClients.DoLongGreet(greetClient);
await StaticClients.DoManyGreetings(greetClient);

var calculatorClient = new CalculatorService.CalculatorServiceClient(channel);

StaticClients.DoSum(calculatorClient);
await StaticClients.DoPrimeNumberDecomposeAsync(calculatorClient);
await StaticClients.DoAverage(calculatorClient);
await StaticClients.DoFindMaximum(calculatorClient);



channel.ShutdownAsync().Wait();
Console.ReadKey();
