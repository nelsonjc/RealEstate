using Microsoft.Extensions.Options;
using RealEstate.Core.Constants;
using RealEstate.Core.Interfaces.Utils;
using RealEstate.Core.Options;
using System.Security.Cryptography;

namespace RealEstate.Core.Services.Utils
{
    public class PasswordService : IPasswordService
    {
        private const string Separator = ".";
        private readonly PasswordOptions _options;
        public PasswordService(IOptions<PasswordOptions> options)
        {
            _options = options.Value;   
        }

        public string GenerateHash(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, _options.SaltSize, _options.Iterations);
            var key = Convert.ToBase64String(algorithm.GetBytes(_options.KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);
            return $"{_options.Iterations}.{salt}.{key}";
        }

        public bool CheckPassword(string hash, string password)
        {
            var parts = hash.Split(Separator);

            if (parts.Length != 3)
                throw new FormatException(MessageConstant.HASH_ERROR);

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations);
            var keyToCkeck = algorithm.GetBytes(_options.KeySize);
            return keyToCkeck.SequenceEqual(key);
        }
    }
}
