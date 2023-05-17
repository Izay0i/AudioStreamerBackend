using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AudioStreamerAPI.Helpers
{
    public class CredentialsHelper
    {
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<Member>();
            return hasher.HashPassword(null!, password);
        }

        public static OperationalStatus VerifyPassword(string password, string typedPassword)
        {
            var hasher = new PasswordHasher<Member>();
            var result = hasher.VerifyHashedPassword(null!, password, typedPassword);
            if (result == PasswordVerificationResult.Success)
            {
                return OperationalStatus.SUCCESS;
            }
            return OperationalStatus.FAILURE;
        }
    }
}
