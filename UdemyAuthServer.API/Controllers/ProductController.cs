using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Entities;
using UdemyAuthServer.Core.Services;

namespace UdemyAuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product,ProductDto> _genericService;

        public ProductController(Core.Services.IGenericService<Product,ProductDto> genericService)
        {
            _genericService = genericService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultInstance(await _genericService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return ActionResultInstance(await _genericService.GetByIdAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _genericService.AddAsync(productDto));
        }
        [HttpPut]
        public async Task<IActionResult>UpdateProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _genericService.UpdateAsync(productDto,productDto.Id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteProduct(int id)
        {
            return ActionResultInstance(await _genericService.RemoveAsync(id));
        }
    }
}
