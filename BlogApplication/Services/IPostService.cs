using BlogApplication.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApplication.Services
{
    public interface IPostService
    {
        Task AddAsync(PostDto postDto);
        IEnumerable<Post> GetAllPosts();
        Task UpdateAsync(Post post);
        Task DeleteAsync(int id);
        IEnumerable<Post> GetByCount(int number);
        Task<Post> GetByIdAsync(int number);
        Task CkEditorPost(IFormFile upload, string fileName);
        byte[] DownloadFile();

    }
}
