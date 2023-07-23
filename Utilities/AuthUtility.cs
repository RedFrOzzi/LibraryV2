using LibraryV2.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryV2.Utilities
{
    public static class AuthUtility
    {
        public static byte[] CreatePasswordHash(string password, string salt)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, string salt)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public static string CreateToken(Reader reader, string jwtKey)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, reader.Id.ToString()),
                new Claim(ClaimTypes.Name, reader.Name),
                new Claim(ClaimTypes.Role, reader.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static RefreshToken GetRefreshToken(Reader reader)
        {
            var refreshToken = new RefreshToken()
            {
                Reader = reader,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            return refreshToken;
        }

        public static RefreshToken UpdateRefreshToken(Reader reader, RefreshToken refreshToken)
        {
            refreshToken.Reader = reader;
            refreshToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            refreshToken.Created = DateTime.UtcNow;
            refreshToken.Expires = DateTime.UtcNow.AddDays(1);

            return refreshToken;
        }

        public static void SetCookieWithRefreshToken(RefreshToken refreshToken, HttpResponse response)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };

            response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
