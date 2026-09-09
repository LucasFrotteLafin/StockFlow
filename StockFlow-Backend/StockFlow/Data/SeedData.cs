using FocusSpace.DatabaseContext;
using FocusSpace.Models;
using FocusSpace.Encrypt;

namespace FocusSpace.Data
{
    public static class SeedData
    {
        public static void Initialize(DataContext context)
        {
            var sha256Hash = PasswordEncryptor.Encrypt("admin123");
            var existingAdmin = context.Users.FirstOrDefault(u => u.Role == "Admin");

            if (existingAdmin == null)
            {
                context.Users.Add(new User
                {
                    Username = "admin",
                    Password = sha256Hash,
                    Role = "Admin"
                });
                context.SaveChanges();
                Console.WriteLine("✓ Usuário admin criado (admin / admin123)");
            }
            else if (existingAdmin.Password != sha256Hash)
            {
                existingAdmin.Password = sha256Hash;
                context.SaveChanges();
                Console.WriteLine("✓ Hash do admin corrigido para SHA-256");
            }
            else
            {
                Console.WriteLine("✓ Admin configurado corretamente");
            }
        }
    }
}
