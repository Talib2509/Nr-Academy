using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs.BlogPostDTO;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyBL.Extensions; 

namespace NrAcademyBL.Services.Concrete
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _repository;
        private readonly IMapper _mapper;

        public BlogPostService(IBlogPostRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BlogPostGetDTO>> GetAsync()
        {
            var posts = await _repository.GetAll()
                                         .Include(x => x.Category)
                                         .ToListAsync();

            return _mapper.Map<IEnumerable<BlogPostGetDTO>>(posts);
        }

        public async Task<BlogPostGetDTO> GetByIdAsync(int id)
        {
            var blogPost = await _repository.GetByIdAsync(id);
            if (blogPost == null) throw new Exception("Belə bir post tapılmadı");

            return _mapper.Map<BlogPostGetDTO>(blogPost);
        }

        public async Task CreateAsync(BlogPostCreateDTO dto, string rootPath)
        {
            var newBlogPost = _mapper.Map<BlogPost>(dto);

            if (dto.ImageFile != null)
            {
                
                newBlogPost.ImageUrl = await dto.ImageFile.UploadAsync(rootPath, "uploads", "blogs");
            }

            await _repository.AddAsync(newBlogPost);
            await _repository.SaveAsync();
        }

        public async Task UpdateAsync(BlogPostUpdateDTO dto, string rootPath)
        {
            var existingPost = await _repository.GetByIdAsync(dto.Id);
            if (existingPost == null) throw new Exception("Yeniləmək istədiyiniz post tapılmadı");

            if (dto.ImageFile != null)
            {
                // Köhnə şəkil  silirik
                if (!string.IsNullOrEmpty(existingPost.ImageUrl))
                {
                    FileExtensions.DeleteFile(existingPost.ImageUrl, rootPath, "uploads", "blogs");
                }
                // Yeni şəkli yükləyirik
                existingPost.ImageUrl = await dto.ImageFile.UploadAsync(rootPath, "uploads", "blogs");
            }

            _mapper.Map(dto, existingPost);
            _repository.Update(existingPost);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(int id, string rootPath)
        {
            var post = await _repository.GetByIdAsync(id);
            if (post == null) throw new Exception("Silmək istədiyiniz post tapılmadı");

            // Post silinəndə şəkli də serverdən silirik
            if (!string.IsNullOrEmpty(post.ImageUrl))
            {
                FileExtensions.DeleteFile(post.ImageUrl, rootPath, "uploads", "blogs");
            }

            _repository.Delete(post);
            await _repository.SaveAsync();
        }
    }
}