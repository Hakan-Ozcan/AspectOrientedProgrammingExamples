using System;
using System.Collections.Generic;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace AopExample
{
    [PSerializable]
    public class CachingAspect : MethodInterceptionAspect
    {
        private static Dictionary<string, object> cache = new Dictionary<string, object>();

        // Önbellek geçerlilik süresi (milisaniye cinsinden)
        public int CacheDuration { get; set; } = 3000; // Varsayılan olarak 3 saniye

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            // Metodun adını ve parametrelerini kullanarak benzersiz bir önbellek anahtarı oluşturuyoruz.
            string cacheKey = $"{args.Method.Name}::{string.Join("_", args.Arguments)}";

            // Önbellekte veriyi arıyoruz.
            if (cache.ContainsKey(cacheKey))
            {
                // Veri önbellekte varsa, önbellekten veriyi alıyoruz.
                Console.WriteLine("Veri önbellekten alındı.");
                args.ReturnValue = cache[cacheKey];
            }
            else
            {
                // Veri önbellekte yoksa, metodu çağırıyoruz ve dönen sonucu önbelleğe ekliyoruz.
                args.Proceed();
                cache[cacheKey] = args.ReturnValue;

                // Veriyi belirli bir süre sonra önbellekten kaldırma (geçerlilik süresi).
                System.Threading.Tasks.Task.Delay(CacheDuration).ContinueWith(_ =>
                {
                    lock (cache)
                    {
                        if (cache.ContainsKey(cacheKey))
                        {
                            cache.Remove(cacheKey);
                            Console.WriteLine("Önbellekten kaldırıldı.");
                        }
                    }
                });
            }
        }
    }

    public class ExampleClass
    {
        [CachingAspect(CacheDuration = 5000)] // Önbellekleme aspect'i burada kullanılıyor. Geçerlilik süresi 5 saniye.
        public int LongRunningMethod(int input)
        {
            // Uzun süren bir hesaplama
            System.Threading.Thread.Sleep(2000);
            return input * 2;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var example = new ExampleClass();

            Console.WriteLine(example.LongRunningMethod(5)); // İlk çağrıda hesaplama yapacak
            Console.WriteLine(example.LongRunningMethod(5)); // İkinci çağrıda önbellekten alacak

            System.Threading.Thread.Sleep(6000); // 6 saniye bekleyelim, önbellekten kaldırılacak.

            Console.WriteLine(example.LongRunningMethod(5)); // Üçüncü çağrıda tekrar hesaplama yapacak
        }
    }
}
