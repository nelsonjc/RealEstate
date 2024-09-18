using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Core.Constants;
using RealEstate.Core.DTOs;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Repository;
using RealEstate.Core.Interfaces.Services;
using RealEstate.Core.Interfaces.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstate.Core.Services.Imp
{
    /// <summary>
    /// Provides authentication services, including user validation and JWT token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration settings for authentication.</param>
        /// <param name="passwordService">Service for handling password operations.</param>
        /// <param name="unitOfWork">Unit of work for accessing repositories.</param>
        public AuthService(
            IConfiguration configuration,
            IPasswordService passwordService,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Authenticates a user based on the provided credentials.
        /// </summary>
        /// <param name="auth">The authentication DTO containing username and password.</param>
        /// <returns>A tuple containing the authenticated user and a message.</returns>
        public async Task<(User? User, string Message)> Authenticate(AuthDto auth)
        {
            (User? User, string Message) resultBad = (null, MessageConstant.AUTH_ERROR);

            try
            {
                var user = (await _unitOfWork.UserRepository.GetByWhereAsync(x => x.Active && x.UserName.ToUpper() == auth.UserName)).FirstOrDefault();

                if (user != null)
                {
                    return _passwordService.CheckPassword(user.Password, auth.Password) ? ((User? User, string Message))(user, MessageConstant.AUTH_OK) : resultBad;
                }
                else
                    return resultBad;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is to be generated.</param>
        /// <returns>A JWT token as a string.</returns>
        public async Task<string> GenerateToken(User user)
        {
            JwtSecurityToken token;

            try
            {
                string secretKey = _configuration["Auth:SecretKey"];
                var simSecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var signinCred = new SigningCredentials(simSecKey, SecurityAlgorithms.HmacSha256);
                var header = new JwtHeader(signinCred);

                // JWT Claims
                var claims = new List<Claim>
            {
                new("Name", user.FullName),
                new("UserName", user.UserName),
            };

                // JWT Payload
                var payload = new JwtPayload(
                    _configuration["Auth:Issuer"],
                    _configuration["Auth:Audience"],
                    claims,
                    DateTime.UtcNow.AddHours(ConfigConstant.HOUR_DIFF),
                    DateTime.UtcNow.AddHours(ConfigConstant.HOUR_DIFF).AddDays(Convert.ToInt32(_configuration["Auth:ExpirationTime"]))
                );

                await Task.CompletedTask;
                token = new JwtSecurityToken(header, payload);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
