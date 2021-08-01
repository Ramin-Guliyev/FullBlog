using BlogApplication.Data;
using BlogApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace BlogApplication.Services
{

    public class PostService : IPostService
    {
        private readonly ApplicationContext _applicationDbContext;
        private IWebHostEnvironment _hostingEnvironment;

        public PostService(ApplicationContext applicationDbContext, IWebHostEnvironment hostingEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task AddAsync(PostDto postDto)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + postDto.FormFile.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), _hostingEnvironment.WebRootPath,
                "uploads", fileName);
            var stream = new FileStream(path, FileMode.Create);
            await postDto.FormFile.CopyToAsync(stream);

            var post = new Post()
            {
                FullPost = postDto.FullPost,
                Title = postDto.Title,
                Description = postDto.Description,
                ImagePath = ("/uploads/" + fileName).ToString()
            };
            await _applicationDbContext.Posts.AddAsync(post);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task CkEditorPost(IFormFile upload, string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), _hostingEnvironment.WebRootPath,
                "uploads", fileName);
            var stream = new FileStream(path, FileMode.Create);
            await upload.CopyToAsync(stream);
        }

        public async Task DeleteAsync(int id)
        {
            var deletedPost = await _applicationDbContext.Posts.FindAsync(id);
            if (deletedPost == null)
                return;

            _applicationDbContext.Posts.Remove(deletedPost);
            await _applicationDbContext.SaveChangesAsync();
        }

        public byte[] DownloadFile()
        {
            var arr = System.IO.File.ReadAllBytes(Path.Combine(
                  Directory.GetCurrentDirectory(), _hostingEnvironment.WebRootPath, "RaminGuliyevResume.pdf"));
            return arr;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _applicationDbContext.Posts.ToList();
        }

        public IEnumerable<Post> GetByCount(int number)
        {
            var posts = _applicationDbContext.Posts.OrderByDescending(o => o.Created).Take(number).ToList();
            return posts;
        }

        public async Task<Post> GetByIdAsync(int number)
        {
            var post = await _applicationDbContext.Posts.FindAsync(number);
            return post;
        }

        public async Task UpdateAsync(Post post)
        {
            var updatedPost = await _applicationDbContext.Posts.FindAsync(post.Id);
            if (updatedPost == null)
                return;

            updatedPost.Description = post.Description;
            updatedPost.FullPost = post.FullPost;
            updatedPost.Title = post.Title;
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
