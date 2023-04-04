using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Dtos;

namespace UdemyAuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<CustomResponse<TokenDto>> CreateToken(LoginDto loginDto);
        Task<CustomResponse<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<CustomResponse<NoDataDto>> RevokeRefreshToken(string refreshToken);

        CustomResponse<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}
