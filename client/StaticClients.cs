using Calculator;
using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    public static class StaticClients
    {
        public static void DoSimpleGreet(GreetingService.GreetingServiceClient client)
        {
            var greeting = new Greeting()
            {
                FirstName = "Oki",
                LastName = "Doki"
            };

            var request = new GreetingRequest()
            {
                Greeting = greeting
            };

            var response = client.Greet(request);
            Console.WriteLine(response.Result);
        }

        public static async Task DoManyGreetings(GreetingService.GreetingServiceClient client)
        {
            var greeting = new Greeting()
            {
                FirstName = "Oki",
                LastName = "Doki"
            };

            var request = new GreetManyTimesRequest()
            {
                Greeting = greeting
            };

            var response = client.GreetManyTimes(request);

            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.Result);
                await Task.Delay(200);
            }
        }

        public static async Task DoLongGreet(GreetingService.GreetingServiceClient client)
        {
            var greeting = new Greeting()
            {
                FirstName = "Oki",
                LastName = "Doki"
            };
            var request = new LongGreetRequest() { Greeting = greeting };
            var stream = client.LongGreet();

            foreach (int i in Enumerable.Range(1, 10))
            {
                await stream.RequestStream.WriteAsync(request);
            }

            await stream.RequestStream.CompleteAsync();

            var response = await stream.ResponseAsync;

            Console.WriteLine(response.Result);
        }

        public static async Task DoGreetEveryone(GreetingService.GreetingServiceClient client)
        {
            var stream = client.GreetEveryone();

            var responseReaderTask = Task.Run(async () =>
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    Console.WriteLine("Received : " + stream.ResponseStream.Current.Result);
                }
            });

            Greeting[] greetings =
            {
                new Greeting() { FirstName = "Oki1", LastName = "Doki1"},
                new Greeting() { FirstName = "Oki2", LastName = "Doki2"},
                new Greeting() { FirstName = "Oki3", LastName = "Doki3"},
                new Greeting() { FirstName = "Oki4", LastName = "Doki4"},
            };

            foreach (var greeting in greetings)
            {
                await stream.RequestStream.WriteAsync(new GreetEveryoneRequest()
                {
                    Greeting = greeting
                });
            }

            await stream.RequestStream.CompleteAsync();
            await responseReaderTask;
        }

        public static void DoSum(CalculatorService.CalculatorServiceClient client)
        {
            var request = new SumRequest()
            {
                FirstNumber = 3,
                SecondNumber = 10,
            };

            var response = client.Sum(request);

            Console.WriteLine(response.Result);
        }

        public static async Task DoPrimeNumberDecomposeAsync(CalculatorService.CalculatorServiceClient client)
        {
            var request = new PrimeNumberDecompositionRequest()
            {
                Number = 1200345100632568345
            };

            var response = client.PrimeNumberDecomposition(request);

            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.Result);
                await Task.Delay(500);
            }
        }

        public static async Task DoAverage(CalculatorService.CalculatorServiceClient client)
        {
            var stream = client.Average();

            var request = new AverageRequest() { Number = 1 };

            for (int i = 1; i < 5; i++)
            {
                await stream.RequestStream.WriteAsync(request);
                request.Number++;
            }

            await stream.RequestStream.CompleteAsync();

            var response = await stream.ResponseAsync;

            Console.WriteLine(response.Result);
        }

        public static async Task DoFindMaximum(CalculatorService.CalculatorServiceClient client)
        {
            var stream = client.FindMaximum();

            var responseReaderTask = Task.Run(async () =>
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    Console.WriteLine("Received : " + stream.ResponseStream.Current.Number);
                }
            });

            int[] numbers = { 1, 5, 3, 6, 2, 20 };

            foreach (var num in numbers)
            {
                await stream.RequestStream.WriteAsync(new FindMaximumRequest()
                {
                    Number = num
                });
            }

            await stream.RequestStream.CompleteAsync();
            await responseReaderTask;
        }
    }
}
