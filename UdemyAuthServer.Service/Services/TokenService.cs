using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Configuration;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Entities;
using UdemyAuthServer.Core.Services;

namespace UdemyAuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomTokenOptions _customTokenOptions;

        public TokenService(UserManager<AppUser> userManager, IOptions<CustomTokenOptions> options)
        {
            _userManager = userManager;
            _customTokenOptions = options.Value;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];

            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
        private IEnumerable<Claim> GetClaim(AppUser appUser, List<String> audiences)
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,appUser.Id),
                new Claim(JwtRegisteredClaimNames.Email,appUser.Email),
                new Claim(ClaimTypes.Name,appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

            };
            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return userList;
        }
        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString())
            };
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return claims;
        }

        public TokenDto CreateToken(AppUser appUser)
        {
            throw new NotImplementedException();
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
