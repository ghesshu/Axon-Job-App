using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Axon_Job_App.Data;
using Microsoft.AspNetCore.Http;

namespace Axon_Job_App.Services;

public class AuthContext(IHttpContextAccessor httpContextAccessor)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? GetUserClaims()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var authHeader = httpContext?.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return null;

        var token = authHeader["Bearer ".Length..].Trim();
        var handler = new JwtSecurityTokenHandler();

        try
        {
            var jwtToken = handler.ReadJwtToken(token);
            return new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, "jwt"));
        }
        catch
        {
            return null;
        }
    }

    public bool IsAuthenticated()
    {
        var claims = GetUserClaims();
        return claims?.Identity?.IsAuthenticated ?? true;
    }

    public async Task CheckRole(DataContext db, string requiredRole)
    {
        var claims = GetUserClaims();
        var role = claims?.FindFirst(ClaimTypes.Role)?.Value;

        if (role != requiredRole)
        {
            await Task.Run(() => throw new UnauthorizedAccessException("Unauthorized Role Access"));
        }
    }

    public async Task CheckPermission(DataContext db, string requiredPermission)
    {
        var claims = GetUserClaims();
        var permissions = claims?.FindFirst("Permissions")?.Value?.Split(',') ?? [];

        if (!permissions.Contains(requiredPermission))
        {
            await Task.Run(() => throw new UnauthorizedAccessException("Unauthorized Permission Access"));
        }
    }

    public string? GetCurrentUserId()
    {
        var claims = GetUserClaims();
        return claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

}