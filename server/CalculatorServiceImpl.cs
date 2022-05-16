using Calculator;
using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Calculator.CalculatorService;

namespace server
{
    public class CalculatorServiceImpl : CalculatorServiceBase
    {
        public override Task<SumResponse> Sum(SumRequest request, ServerCallContext context)
        {
            int result = request.FirstNumber + request.SecondNumber;
            return Task.FromResult(new SumResponse() { Result = result });
        }

        public override async Task PrimeNumberDecomposition(PrimeNumberDecompositionRequest request, IServerStreamWriter<PrimeNumberDecompositionResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("The server received the request : ");
            Console.WriteLine(request.ToString());

            long k = 2;
            long N = request.Number;

            while (N > 1)
            {
                if (N % k == 0)
                {
                    await responseStream.WriteAsync(new PrimeNumberDecompositionResponse() { Result = k });
                    N = N / k;
                }
                else
                {
                    k = k + 1;
                }
            }
        }

        public override async Task<AverageResponse> Average(IAsyncStreamReader<AverageRequest> requestStream, ServerCallContext context)
        {
            Console.WriteLine("The server received the request : ");
            double result = 0.0;
            int count = 0;

            while(await requestStream.MoveNext())
            {
                result += requestStream.Current.Number;
                count++;
            }

            return new AverageResponse() { Result = result / count };
        }

        public override async Task FindMaximum(IAsyncStreamReader<FindMaximumRequest> requestStream, IServerStreamWriter<FindMaximumResponse> responseStream, ServerCallContext context)
        {
            int? maximum = null;
            
            while (await requestStream.MoveNext())
            {
                if (maximum == null || maximum < requestStream.Current.Number)
                {
                    maximum = requestStream.Current.Number;

                    await responseStream.WriteAsync(new FindMaximumResponse() { Number = maximum.Value });
                }
            }
        }
    }
}
