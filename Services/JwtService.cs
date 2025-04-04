using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Axon_Job_App.Services;

public class JwtService
{
    private static IConfiguration? _config;

    public static void Initialize(IConfiguration configuration)
    {
        _config = configuration;
    }

    public static string GenerateToken(string userId, string role, List<string> permissions)
    {
        if (_config == null)
            throw new InvalidOperationException("JWT configuration is not initialized");
        var jwtKey = _config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key is not configured.");
        var key = Encoding.UTF8.GetBytes(jwtKey);
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, role),
            new("Permissions", string.Join(",", permissions))
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiryInMinutes"])),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
