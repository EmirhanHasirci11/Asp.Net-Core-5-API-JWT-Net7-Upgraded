using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Repositories;
using UdemyAuthServer.Core.Services;
using UdemyAuthServer.Core.UnitOfWork;

namespace UdemyAuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }
        public async Task<CustomResponse<TDto>> AddAsync(TDto dto)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(dto);

            await _genericRepository.AddAsync(newEntity);

            await _unitOfWork.CommitAsync();

            var dtoEntity = ObjectMapper.Mapper.Map<TDto>(newEntity);
            return CustomResponse<TDto>.Success(dtoEntity, 200);
        }

        public async Task<CustomResponse<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());

            return CustomResponse<IEnumerable<TDto>>.Success(entities, 200);
        }

        public async Task<CustomResponse<TDto>> GetByIdAsync(int id)
        {

            var entity = await _genericRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return CustomResponse<TDto>.Fail("Id not found", 404, true);
            }

            return CustomResponse<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(entity), 200);
        }

        public async Task<CustomResponse<NoDataDto>> RemoveAsync(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            if(entity == null)
            {
                return CustomResponse<NoDataDto>.Fail("Id not Found", 404,true);
            }
            _genericRepository.Remove(entity);

            await _unitOfWork.CommitAsync();

            return CustomResponse<NoDataDto>.Success(204);


        }

        public async Task<CustomResponse<NoDataDto>> UpdateAsync(TDto dto,int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return CustomResponse<NoDataDto>.Fail("Id not Found", 404, true);
            }
            var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            _genericRepository.Update(updateEntity);
            await _unitOfWork.CommitAsync();
            return CustomResponse<NoDataDto>.Success(204);

        }

        public async Task<CustomResponse<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> expression)
        {
            var list = _genericRepository.Where(expression);

            return CustomResponse<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>( await list.ToListAsync()), 200);

        }
    }
}
