using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAuthServer.Core.Services
{
    public interface IGenericService<TEntity,TDto> where TEntity:class where TDto : class
    {
        Task<CustomResponse<TDto>> GetByIdAsync(int id);
        Task<CustomResponse<IEnumerable<TDto>>> GetAllAsync();
        Task<CustomResponse<IEnumerable<TDto>>>Where(Expression<Func<TEntity, bool>> expression);
        Task<CustomResponse<TDto>> AddAsync(TEntity entity);
        Task<CustomResponse<NoDataDto>> RemoveAsync(TEntity entity);
        Task<CustomResponse<NoDataDto>> UpdateAsync(TEntity entity);

    }
}
