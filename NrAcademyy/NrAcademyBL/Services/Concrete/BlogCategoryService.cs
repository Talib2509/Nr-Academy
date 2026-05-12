using AutoMapper;
using NrAcademyBL.DTOs.BlogCategoryDTO;
using NrAcademyBL.Extensions.Caching;
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
    public class BlogCategoryService: IBlogCategoryService
    {
        private readonly IBlogCategoryRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        public BlogCategoryService(IBlogCategoryRepository repo, IMapper mapper, ICacheService cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
        }
        public async Task CreateAsync(BlogCategoryCreateDTO dto)
        {
            var category = _mapper.Map<BlogCategory>(dto);
            await _repo.AddAsync(category);
           

            await _cache.RemoveAsync("blogcategories_all");
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Silinecek kateqoriya tapılmadı");

            _repo.DeleteAsync(category);
            await _repo.SaveAsync();

            await _cache.RemoveAsync("blogcategories_all");
            await _cache.RemoveAsync($"blogcategory_{id}");
        }
        public async Task<List<BlogCategoryGetDTO>> GetAllAsync()
        {
            var key = "blogcategories_all";

            var cached = await _cache.GetAsync<List<BlogCategoryGetDTO>>(key);
            if (cached != null)
                return cached;

            var categories = await _repo.GetAllAsync();
            var mapped = _mapper.Map<List<BlogCategoryGetDTO>>(categories);

            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(30));

            return mapped;
        }

        public async Task<BlogCategoryGetDTO> GetByIdAsync(int id)
        {
            var key = $"blogcategory_{id}";

            var cached = await _cache.GetAsync<BlogCategoryGetDTO>(key);
            if (cached != null)
                return cached;

            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Kateqoriya tapılmadı");

            var mapped = _mapper.Map<BlogCategoryGetDTO>(category);

            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(30));

            return mapped;
        }

        public async Task UpdateAsync(int id, BlogCategoryUpdateDTO dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Yenilenecek kateqoriya tapılmadı");

            _mapper.Map(dto, category);
            _repo.UpdateAsync(category);
            await _repo.SaveAsync();

            await _cache.RemoveAsync("blogcategories_all");
            await _cache.RemoveAsync($"blogcategory_{id}");
        }
    }
}
