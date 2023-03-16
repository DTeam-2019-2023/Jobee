using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Jobee_Lib
{
    #region JWT Token Manager
    public class JwtTokenManager
    {
        private static string _secretKey = default!;

        public static string SecretKey {
            set
            {
                if(instance!= null)
                {
                    throw new InvalidOperationException("The secretKey can not be change after the instance is created.");
                }
                _secretKey = value;
            }
        }
        public JwtTokenManager(string secretKey)
        {
            _secretKey = secretKey;
        }

        private static JwtTokenManager instance;
        
        public static JwtTokenManager Instance
        {
            get {
                lock (typeof(JwtTokenManager)) {
                    // Create the instance if it does not exist yet
                    if (instance == null)
                    {
                        instance = new (_secretKey);
                    }
                    // Return the instance
                    return instance;
                }
            }
        }

        public string GenerateJwtToken(string issuer, string audience, DateTime? expires = null, params Claim[] claims)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims,
                expires: DateTime.Now.AddDays(60d),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_secretKey);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
                return principal;
            }
            catch
            {
                // return null if validation fails
                return null!;
            }
        }
        public bool ValidateJwtToken(string token)
        {
            var principal = GetPrincipalFromToken(token);
            return principal != null;
        }

    }
    #endregion
}
