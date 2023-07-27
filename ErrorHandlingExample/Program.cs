using System;
using PostSharp.Aspects;

namespace AopExample
{
    // Aspect olarak işaretlemek için [Serializable] ve [Aspect] niteliklerini kullanıyoruz.
    [Serializable]
    public class ErrorHandlingAspect : OnExceptionAspect
    {
        // Metot içinde bir hata oluştuğunda çalışacak kodları burada tanımlıyoruz.
        public override void OnException(MethodExecutionArgs args)
        {
            // Hata detaylarını alıyoruz.
            Exception ex = args.Exception;
            string methodName = args.Method.Name;

            // Hata durumunda uygun bir geri bildirim sağlayabiliriz. Burada sadece konsola yazdırıyoruz.
            Console.WriteLine($"Error occurred in method '{methodName}': {ex.Message}");
            // Ayrıca hata yönetimiyle ilgili başka işlemler de burada yapılabilir.

            // Metodu devam ettirmek istemiyorsak istisna nesnesini boşaltarak işlemi sonlandırabiliriz.
            args.Exception = null;
        }
    }

    // Aspect'i bir metota uyguluyoruz.
    public class ExampleClass
    {
        [ErrorHandlingAspect] // Hata yönetimi aspect'i burada kullanılıyor.
        public void Divide(int a, int b)
        {
            int result = a / b;
            Console.WriteLine($"Result: {result}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var example = new ExampleClass();

            try
            {
                example.Divide(10, 0); // Sıfıra bölme hatası oluşacak
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }
        }
    }
}
