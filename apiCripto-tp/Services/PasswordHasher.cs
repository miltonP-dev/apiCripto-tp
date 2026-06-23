using System.Security.Cryptography;

namespace apiCripto_tp.Services
{
    public class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string password, string stored)
        {
            var partes = stored.Split('.', 2);
            if (partes.Length != 2) return false;

            byte[] salt = Convert.FromBase64String(partes[0]);
            byte[] hashEsperado = Convert.FromBase64String(partes[1]);
            byte[] hashActual = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

            return CryptographicOperations.FixedTimeEquals(hashActual, hashEsperado);
        }
    }
}
