using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TransactEase.DataLayer;
using TransactEase.DTO;
using TransactEase.Enums;
using TransactEase.Models;

namespace TransactEase.BusinessLayer;

public class AuthHandler(AuthDAL authDal, IMemoryCache memoryCache)
{
    private readonly AuthDAL _authDal = authDal;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<UserResponse> LoginAsync(LoginModel login)
    {
        try
        {
            var user = await _authDal.GetUserByUsernameAsync(login.Username);

            if (user == null || user.Password != login.Password)
            {
                return new UserResponse { Message = "Invalid credentials", Status = StatusEnum.ERROR };
            }

            var sessionId = Guid.NewGuid().ToString();
            var connectionString = login.ConnectionString ?? GetConnectionStringForOrganization(login.OrganizationIdentifier);

            var session = new SessionModel
            {
                SessionId = sessionId,
                ConnectionString = connectionString
            };

            _memoryCache.Set(sessionId, session, TimeSpan.FromHours(1));

            var token = GenerateJwtToken(sessionId);

            return new UserResponse { Message = "Login successful", Status = StatusEnum.SUCCESS, Data = new { Token = token } };
        }
        catch (Exception e)
        {
            return new UserResponse { Message = e.Message, Status = StatusEnum.ERROR };
        }
    }

    private string GetConnectionStringForOrganization(string organizationIdentifier)
    {
        // This is a placeholder. In a real application, you would have a mechanism
        // to resolve the connection string based on the organization identifier.
        return "your_default_connection_string";
    }

    private string GenerateJwtToken(string sessionId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("your_super_secret_key_that_is_at_least_32_bytes_long"); // Replace with a secure key
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("sessionId", sessionId) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
