using AudioStreamerAPI.Models;
using AudioStreamerAPI.Constants;
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
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Ok,
                    Message = "Successfully verified password.",
                };
            }
            return new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.BadRequest,
                Message = "Failed to verify password.",
            };
        }
    }
}
