using System;
using PostSharp.Aspects;

namespace MonitoringExample
{
    // İzleme ve İzleme için bir Aspect tanımlıyoruz.
    [Serializable]
    public class MonitoringAttribute : OnMethodBoundaryAspect
    {
        private DateTime startTime;
        private int invocationCount;

        public override void OnEntry(MethodExecutionArgs args)
        {
            startTime = DateTime.Now;
            invocationCount++;
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var endTime = DateTime.Now;
            var elapsedTime = endTime - startTime;
            Console.WriteLine($"Method '{args.Method.Name}' took {elapsedTime.TotalMilliseconds} milliseconds to execute.");
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine($"An exception occurred in method '{args.Method.Name}': {args.Exception.Message}");
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            Console.WriteLine($"Method '{args.Method.Name}' was invoked {invocationCount} times.");
        }
    }

    class Program
    {
        // İzleme ve İzleme için Aspect'i işleve uyguluyoruz.
        [Monitoring]
        static int CalculateSum(int[] numbers)
        {
            if (numbers == null)
            {
                throw new ArgumentNullException(nameof(numbers));
            }

            int sum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }
            return sum;
        }

        static void Main(string[] args)
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // İşlevi çağırarak izleme ve izleme bilgilerini alıyoruz.
            int result = CalculateSum(numbers);
            Console.WriteLine($"Result: {result}");
        }
    }
}
