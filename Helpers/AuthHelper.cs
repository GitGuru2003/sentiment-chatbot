using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace chatbot.Helpers
{
  public class AuthHelper
  {
    private readonly IConfiguration _config;
    public AuthHelper(IConfiguration config)
    {
      _config = config;
    }

    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
      string? passwordSaltString = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);

      byte[] passwordHash = KeyDerivation.Pbkdf2(
        password: password,
        salt: Encoding.ASCII.GetBytes(passwordSaltString),
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 10000,
        numBytesRequested: 32
      );
      return passwordHash;
    }

    public string CreateToken(int userId, string role)
    {
      // Claim[] claims = new Claim[]{
      //   new Claim("userId", userId.ToString()) // "userId" is the identifier for the claim and userId.ToString() is the value of the claim.
      // };
      Claim[] claims = new Claim[]
      {
          new Claim("userId", userId.ToString()),
          new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
          new Claim(ClaimTypes.Role, role)
      };


      string? tokenKey = _config.GetSection("AppSettings:TokenKey").Value;

      if (tokenKey != null)
      {
        SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(tokenKey)
      );

        SigningCredentials credentials = new SigningCredentials(
          symmetricKey, SecurityAlgorithms.HmacSha256
        );

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
        {
          Subject = new ClaimsIdentity(claims),
          SigningCredentials = credentials,
          Expires = DateTime.UtcNow.AddDays(1)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        string tokenString = tokenHandler.WriteToken(token);

        return tokenString;
      }

      throw new Exception("Token key is not configured in AppSettings.");

    }

  }
}