using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using blogest.application.DTOs.responses;
using blogest.application.Interfaces.services;
using blogest.infrastructure.Identity;
using blogest.infrastructure.persistence;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;

namespace blogest.infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly BlogCommandContext _context;
    public JwtService(BlogCommandContext context)
    {
        DotNetEnv.Env.Load();
        _secret = Environment.GetEnvironmentVariable("SECRET");
        _issuer = Environment.GetEnvironmentVariable("ISSUER");
        _context = context;
    }
    public string GenrateToken(Guid userId, string userName, IEnumerable<string> roles, int expiryMinutes = 60)
    {

        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub,userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName,userName),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task AddRefreshTokenToDb(string refreshToken, Guid userId)
    {
        await _context.RefreshTokens.AddAsync(new RefreshToken
        {
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = userId
        });
        await _context.SaveChangesAsync();
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    public async Task<RefreshTokenResponse> RotateRefreshTokenAsync(string oldToken, DateTime newExpiryDate)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(
            rt => rt.Token == oldToken
        );
        if (refreshToken == null || refreshToken.RevokedAt != null || refreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            return new RefreshTokenResponse(null, null, false);
        }

        if (refreshToken != null || refreshToken.RevokedAt == null)
        {

            string newRefreshToken = GenerateRefreshToken();
            refreshToken.Revoke(newRefreshToken);
            await AddRefreshTokenToDb(refreshToken.Token,refreshToken.UserId);

            await _context.SaveChangesAsync();

            AppUser user = await _context.DomainUsers.FirstOrDefaultAsync(u => u.Id == refreshToken.UserId);
            if (user == null)
            {
                return new RefreshTokenResponse(null, null, false);
            }
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => role.Name)
                .ToListAsync();

            string accessToken = GenrateToken(user.Id, user.UserName, roles);
            return new RefreshTokenResponse(accessToken: accessToken, refreshToken: newRefreshToken, success: true);
        }
        return new RefreshTokenResponse(success: false, accessToken: null, refreshToken: null);
    }
    public async Task<bool> IsRefreshTokenValid(string refreshToken)
    {
        return await _context.RefreshTokens.AnyAsync(rt => rt.Token == refreshToken && rt.IsActive);
    }
}