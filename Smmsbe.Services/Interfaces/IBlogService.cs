using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Interfaces
{
    public interface IBlogService
    {
        Task<Blog> GetById(int id);

        Task<BlogResponse> AddBlogAsync(AddBlogRequest request);

        Task<List<BlogResponse>> SearchBlogAsync(SearchBlogRequest request);

        Task<BlogResponse> UpdateBlogAsync(UpdateBlogRequest request);

        Task<bool> DeleteBlogAsync(int id);

        string GetImageFolder();

    }
}
