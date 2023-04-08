using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Configuration;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Entities;
using UdemyAuthServer.Core.Repositories;
using UdemyAuthServer.Core.Services;
using UdemyAuthServer.Core.UnitOfWork;

namespace UdemyAuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _genericRepository;

        public AuthenticationService(IOptions<List<Client>> optionClient, ITokenService tokenService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> genericService)
        {
            _clients = optionClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _genericRepository = genericService;
        }

        public async Task<CustomResponse<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto is null) throw new ArgumentNullException(nameof(loginDto));
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) return CustomResponse<TokenDto>.Fail("Email or Password is wrong", 400, true);

            if (! await _userManager.CheckPasswordAsync(user, loginDto.Password)) return CustomResponse<TokenDto>.Fail("Email or Password is wrong", 400, true);

            var token = await _tokenService.CreateToken(user);

            var userRefreshToken = await _genericRepository.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            if(userRefreshToken is null)
            {
                await _genericRepository.AddAsync(new UserRefreshToken{ UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration= token.RefreshTokenExpiration;
            }
            await _unitOfWork.CommitAsync();

            return CustomResponse<TokenDto>.Success(token, 200);



        }

        public CustomResponse<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);
            if(client is null)
            {
                return CustomResponse<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
            }
            var token =_tokenService.CreateTokenByClient(client);
            return CustomResponse<ClientTokenDto>.Success(token, 200);
        }

        public async Task<CustomResponse<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var refreshTokenExist = await _genericRepository.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if(refreshTokenExist is null)
            {
                return CustomResponse<TokenDto>.Fail("Refresh token not found", 404, true);
            }
            var user = await _userManager.FindByIdAsync(refreshTokenExist.UserId);
            if(user is null)
            {
                return CustomResponse<TokenDto>.Fail("User Id not found", 404, true);
            }
            var token = await _tokenService.CreateToken(user);
            refreshTokenExist.Expiration = token.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return CustomResponse<TokenDto>.Success(token, 200);
        }

        public async Task<CustomResponse<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var refreshTokenExist = await _genericRepository.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if(refreshTokenExist is null)
            {
                return CustomResponse<NoDataDto>.Fail("Refresh token not found", 404, true);
            }
            _genericRepository.Remove(refreshTokenExist);
            await _unitOfWork.CommitAsync();

            return CustomResponse<NoDataDto>.Success(200);
        }
    }
}
