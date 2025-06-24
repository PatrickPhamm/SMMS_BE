using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Smmsbe.Repositories;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Common;
using Smmsbe.Services.Enum;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly AppSettings _appSettings;

        public BlogService(IBlogRepository blogRepository, AppSettings appSettings)
        {
            _blogRepository = blogRepository;
            _appSettings = appSettings;
        }

        public async Task<Blog> GetById(int id)
        {
            var entity = await _blogRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<BlogResponse> AddBlogAsync(AddBlogRequest request)
        {
            var newBlog = new Blog
            {
                ManagerId = request.ManagerId,
                Title = request.Title,
                Content = request.Content,
                DatePosted = request.DatePosted,
                Thumbnail = request.Thumbnail,
                Category = (int)request.Category
            };

            var added = await _blogRepository.Insert(newBlog);

            return new BlogResponse
            {
                ManagerId = added.ManagerId,
                Title = added.Title,
                Content = added.Content,
                DatePosted = added.DatePosted,
                Thumbnail = GetBlogImageUrl(added.Thumbnail),
                Category = ((BlogCategoryType)added.Category).ToString()
            };
        }

        public async Task<List<BlogResponse>> SearchBlogAsync(SearchBlogRequest request)
        {
            var query = _blogRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.BlogId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.Title) && s.Title.Contains(request.Keyword)) ||
                            (!string.IsNullOrEmpty(s.Category.ToString()) && s.Category.ToString().Contains(request.Keyword)));

            var Blogs = await query.Select(n => new BlogResponse
            {
                BlogId = n.BlogId,
                ManagerId = n.ManagerId,
                Title = n.Title,
                Content = n.Content,
                DatePosted = n.DatePosted,
                Thumbnail = n.Thumbnail,
                Category = ((BlogCategoryType)n.Category).ToString()
            }).ToListAsync();

            return Blogs;
        }

        public async Task<BlogResponse> UpdateBlogAsync(UpdateBlogRequest request)
        {
            var updateBlog = await _blogRepository.GetById(request.BlogId);
            if (updateBlog == null) throw AppExceptions.NotFoundId();

            //updateBlog.ManagerId = request.ManagerId;
            updateBlog.Title = request.Title;
            updateBlog.Content = request.Content;
            updateBlog.DatePosted = request.DatePosted;
            updateBlog.Thumbnail = request.Thumbnail;

            await _blogRepository.Update(updateBlog);

            return new BlogResponse
            {
                Title = updateBlog.Title,
                Content = updateBlog.Content,
                DatePosted = updateBlog.DatePosted,
                Thumbnail = GetBlogImageUrl(updateBlog.Thumbnail),
                Category = ((BlogCategoryType)updateBlog.Category).ToString()
            };
        }

        public async Task<bool> DeleteBlogAsync(int id)
        {
            try
            {
                var blog = await _blogRepository.GetById(id);
                if (blog == null) throw AppExceptions.NotFoundId();

                await _blogRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        const string ImageFolder = "files/blogs/";

        public string GetImageFolder()
        {
            return ImageFolder;
        }

        private string GetBlogImageUrl(string thumbnail)
        {
            if (string.IsNullOrEmpty(thumbnail)) return "";

            if (thumbnail.StartsWith("http://") || thumbnail.StartsWith("https://"))
                return thumbnail;

            // Assuming _appSettings.ApplicationUrl is the base URL of your application
            return $"{_appSettings.ApplicationUrl}/{ImageFolder}/{thumbnail}";
        }
    }
}
