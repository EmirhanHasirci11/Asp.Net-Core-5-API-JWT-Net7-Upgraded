using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace UdemyAuthServer.API.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult ActionResultInstance<T>(CustomResponse<T> response) where T : class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode,
            };
        }
    }
}
