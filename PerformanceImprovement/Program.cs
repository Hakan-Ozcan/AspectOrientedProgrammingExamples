using System;
using PostSharp.Aspects;

namespace PerformanceImprovementExample
{
    // Performansı ölçmek için bir Aspect tanımlıyoruz.
    [Serializable]
    public class MeasurePerformanceAttribute : OnMethodBoundaryAspect
    {
        private DateTime startTime;

        public override void OnEntry(MethodExecutionArgs args)
        {
            startTime = DateTime.Now;
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var endTime = DateTime.Now;
            var elapsedTime = endTime - startTime;
            Console.WriteLine($"Method '{args.Method.Name}' took {elapsedTime.TotalMilliseconds} milliseconds to execute.");
        }
    }

    class Program
    {
        // Performansı ölçmek istediğimiz işlevimiz.
        [MeasurePerformance]
        static int CalculateSum(int[] numbers)
        {
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

            // İşlevi çağırarak performans süresini ölçüyoruz.
            int result = CalculateSum(numbers);
            Console.WriteLine($"Result: {result}");
        }
    }
}
