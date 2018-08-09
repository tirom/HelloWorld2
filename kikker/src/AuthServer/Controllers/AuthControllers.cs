namespace AuthServer.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using ApiGateway.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private IOptionsSnapshot<ConfigServerData> IConfigServerData { get; set; }

        public AuthController(IOptionsSnapshot<ConfigServerData> configServerData)
        {
            if (configServerData != null)
                IConfigServerData = configServerData;
        }

        [HttpGet]
        public IActionResult Get(string name, string pwd)
        {
            if (name == "catcher" && pwd == "123")
            {
                string Secret = string.Empty;
                string Iss = string.Empty;
                string Aud = string.Empty;
                if (IConfigServerData != null && IConfigServerData.Value != null)
                {
                    var data = IConfigServerData.Value;
                    Secret = data.Audience.Secret ?? "Not returned";
                    Iss = data.Audience.Iss ?? "Not returned";
                    Aud = data.Audience.Aud ?? "Not returned";

                }
                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                    new Claim("username", name),
                    //add user roles
                    new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, Iss)
                };

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
                var jwt = new JwtSecurityToken(
                    issuer:Iss,
                    audience: Aud,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
                };

                return Json(responseJson);
            }
            else if (name == "cat" && pwd == "123")
            {
                string Secret = string.Empty;
                string Iss = string.Empty;
                string Aud = string.Empty;
                if (IConfigServerData != null && IConfigServerData.Value != null)
                {
                    var data = IConfigServerData.Value;
                    Secret = data.Audience.Secret ?? "Not returned";
                    Iss = data.Audience.Iss ?? "Not returned";
                    Aud = data.Audience.Aud ?? "Not returned";                    
                }
                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, name),
                    new Claim("username", name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                    new Claim(ClaimTypes.Role, "Customer", ClaimValueTypes.String, Iss)
                };

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
                var jwt = new JwtSecurityToken(                    
                    issuer: Iss,
                    audience: Aud,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
                };

                return Json(responseJson);
            }
            else
            {
                return Json("");
            }
        }
    }

}