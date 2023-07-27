using System;
using System.Security.Principal;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace AopExample
{
    // Aspect olarak işaretlemek için [Serializable] ve [Aspect] niteliklerini kullanıyoruz.
    [Serializable]
    
    public class AuthorizationAspect : OnMethodBoundaryAspect
    {
        // İzin verilen rolleri tutan bir dizi oluşturuyoruz.
        private string[] allowedRoles;

        // İzin verilen rolleri belirtmek için constructor kullanıyoruz.
        public AuthorizationAspect(params string[] roles)
        {
            allowedRoles = roles;
        }

        // Metodun başında çalışacak kodları burada tanımlıyoruz.
        public override void OnEntry(MethodExecutionArgs args)
        {
            // Kullanıcının rolünü alıyoruz.
            IPrincipal user = System.Threading.Thread.CurrentPrincipal;
            string userRole = user.Identity.Name;

            // İzin verilen roller içinde kullanıcının rolünün olup olmadığını kontrol ediyoruz.
            if (!Array.Exists(allowedRoles, role => role.Equals(userRole, StringComparison.InvariantCultureIgnoreCase)))
            {
                Console.WriteLine($"Unauthorized access! User with role '{userRole}' is not allowed to call '{args.Method.Name}'.");
                throw new UnauthorizedAccessException("Unauthorized access!");
            }
        }
    }

    // Aspect'i bir metota uyguluyoruz.
    public class ExampleClass
    {
        [AuthorizationAspect("Admin")] // Yetkilendirme aspect'i burada kullanılıyor.
        public void SecureMethod()
        {
            Console.WriteLine("SecureMethod is running.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Simüle edilmiş kullanıcı rolü
            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Admin"), new string[] { "Admin" });

            var example = new ExampleClass();
            example.SecureMethod();

            // Simüle edilmiş kullanıcı rolü
            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("User"), new string[] { "User" });

            example.SecureMethod(); // Yetkilendirme hatası oluşacak
        }
    }
}

