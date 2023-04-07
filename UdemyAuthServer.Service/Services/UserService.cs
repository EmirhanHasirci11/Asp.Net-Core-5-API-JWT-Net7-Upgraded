using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Entities;
using UdemyAuthServer.Core.Services;

namespace UdemyAuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CustomResponse<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new AppUser {Email= createUserDto.Email,UserName=createUserDto.UserName,};
           var result= await _userManager.CreateAsync(user,createUserDto.Password);
            if (!result.Succeeded)
            {
                var errors=result.Errors.Select(x=>x.Description).ToList();
                return CustomResponse<AppUserDto>.Fail(new ErrorDto(errors, true), 400);
            }
            return CustomResponse<AppUserDto>.Success(ObjectMapper.Mapper.Map<AppUserDto>(user),200);
        }

        public async Task<CustomResponse<AppUserDto>> GetUserByNameAsync(string userName)
        {
            var user =await _userManager.FindByNameAsync(userName);
            if(user is null)
            {
                return CustomResponse<AppUserDto>.Fail("Username not found", 404, true);
            }
            return CustomResponse<AppUserDto>.Success(ObjectMapper.Mapper.Map<AppUserDto>(user), 200);
        }
    }
}
