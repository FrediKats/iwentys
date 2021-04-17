using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Iwentys.Domain
{
    public class UniversitySystemUserCredential
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string PasswordHash { get; set; }

        public bool IsStringSame(string plainText)
        {
            return string.Equals(PasswordHash, GetHashString(plainText), StringComparison.InvariantCulture);
        }

        public static Func<UniversitySystemUserCredential, Boolean> IsCredentialMatch(int id, string password)
        {
            var hash = GetHashString(password);

            return user => user.Id == id && user.PasswordHash == hash;
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}