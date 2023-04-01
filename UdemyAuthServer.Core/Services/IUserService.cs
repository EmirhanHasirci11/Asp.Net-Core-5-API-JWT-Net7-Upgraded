using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Dtos;

namespace UdemyAuthServer.Core.Services
{
    public interface IUserService
    {
        Task<CustomResponse<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<CustomResponse<AppUserDto>> GetUserByNameAsync(string userName);
    }
}
