using System;
using PostSharp.Aspects;

namespace AopExample
{
    // Aspect olarak işaretlemek için [Serializable] ve [Aspect] niteliklerini kullanıyoruz.
    [Serializable]
    public class LoggingAspect : OnMethodBoundaryAspect
    {
        // Metodun başında çalışacak kodları burada tanımlıyoruz.
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine($"Method '{args.Method.Name}' started at {DateTime.Now}");
        }

        // Metodun sonunda çalışacak kodları burada tanımlıyoruz.
        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine($"Method '{args.Method.Name}' completed at {DateTime.Now}");
        }
    }

    // Aspect'i bir metota uyguluyoruz.
    public class ExampleClass
    {
        [LoggingAspect] // Günlükleme aspect'i burada kullanılıyor.
        public void MyMethod()
        {
            Console.WriteLine("MyMethod is running.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var example = new ExampleClass();
            example.MyMethod();
        }
    }
}
