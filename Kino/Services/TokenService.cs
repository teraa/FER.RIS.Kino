using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Kino.Services;

public class TokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly SymmetricSecurityKey _signingKey;

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;

        byte[] bytes = Encoding.ASCII.GetBytes(_jwtOptions.SigningKey);
        _signingKey = new SymmetricSecurityKey(bytes);
    }

    public string CreateToken(int userId, bool isAdmin)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
        };

        if (isAdmin)
            claims.Add(new Claim(AppClaim.Admin, true.ToString()));

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow + _jwtOptions.TokenLifetime,
            SigningCredentials = new SigningCredentials(
                key: _signingKey,
                algorithm: SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
