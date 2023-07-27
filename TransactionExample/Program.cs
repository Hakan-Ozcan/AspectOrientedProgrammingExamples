using System;
using PostSharp.Aspects;

namespace AopOrnek
{
    [Serializable]
    public class TransactionAspect : OnMethodBoundaryAspect
    {
        private bool isTransactionOpen = false;

        public override void OnEntry(MethodExecutionArgs args)
        {
            // Transaksiyon başlatma işlemi
            Console.WriteLine("Transaksiyon başlatıldı.");
            isTransactionOpen = true;
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            // Hata durumunda transaksiyonu geri al
            if (isTransactionOpen)
            {
                Console.WriteLine("Transaksiyon tamamlandı.");
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            // Hata durumunda transaksiyonu geri al
            Console.WriteLine("Hata nedeniyle transaksiyon geri alındı.");
        }
    }

    public class BankaHesabi
    {
        public decimal Bakiye { get; private set; }

        [TransactionAspect] // Özel aspect'i burada kullanıyoruz.
        public void ParaYatir(decimal miktar)
        {
            Bakiye += miktar;
            Console.WriteLine($"Yatırılan miktar: {miktar}, Yeni bakiye: {Bakiye}");
        }

        [TransactionAspect] // Özel aspect'i burada kullanıyoruz.
        public void ParaCek(decimal miktar)
        {
            if (Bakiye >= miktar)
            {
                Bakiye -= miktar;
                Console.WriteLine($"Çekilen miktar: {miktar}, Yeni bakiye: {Bakiye}");
            }
            else
            {
                Console.WriteLine("Yetersiz bakiye.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var hesap = new BankaHesabi();

            hesap.ParaYatir(1000);
            hesap.ParaCek(500);
            hesap.ParaCek(700); // Yetersiz bakiye, işlem geri alınacak
        }
    }
}

