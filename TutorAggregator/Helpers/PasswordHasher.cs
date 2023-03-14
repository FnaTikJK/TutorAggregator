using System.Security.Cryptography;

namespace TutorAggregator.Helpers
{
    public class PasswordHasher
    {
        public static string ComputeHash(string password)
        {
            byte[] passwordSalt;
            byte[] passwordHash;
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            byte[] hashBytes = new byte[128+64];
            Array.Copy(passwordSalt, 0, hashBytes, 0, 128);
            Array.Copy(passwordHash, 0, hashBytes, 128, 64);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool ComparePasswordWithHashed(string hashedPassword, string inputPassword)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[128];
            byte[] passwordHash = new byte[64];
            Array.Copy(hashedPasswordBytes, 0, salt, 0, 128);
            Array.Copy(hashedPasswordBytes, 128, passwordHash, 0, 64);

            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(inputPassword));                
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
