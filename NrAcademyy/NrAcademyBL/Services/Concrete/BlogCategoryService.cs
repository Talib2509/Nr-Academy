using AutoMapper;
using NrAcademyBL.DTOs.BlogCategoryDTO;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete
{
    public class BlogCategoryService(IBlogCategoryRepository _repo, IMapper _mapper) : IBlogCategoryService
    {
        public async Task CreateAsync(BlogCategoryCreateDTO dto)
        {
            var category = _mapper.Map<BlogCategory>(dto);
            await _repo.AddAsync(category);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) throw new Exception("Silinecek kateqoriya tapılmadı ");

            _repo.Delete(category);
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<BlogCategoryGetDTO>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<BlogCategoryGetDTO>>(categories);
        }

        public async Task<BlogCategoryGetDTO> GetByIdAsync(int id)
        {

            var category = await _repo.GetByIdAsync(id);
            if (category == null) throw new Exception("Kateqoriya tapılmadı");
            return _mapper.Map<BlogCategoryGetDTO>(category);
        }

        public async Task UpdateAsync(int id, BlogCategoryUpdateDTO dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) throw new Exception("Yenilenecek kateqoriya tapılmadı");

            _mapper.Map(dto, category);
            _repo.Update(category);
            await _repo.SaveAsync();
        }
    }
}
