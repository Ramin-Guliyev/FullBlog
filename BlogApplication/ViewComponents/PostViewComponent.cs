using BlogApplication.Models;
using BlogApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.ViewComponents
{
    public class PostViewComponent : ViewComponent
    {
        private readonly IPostService _postService;
        private IMemoryCache _memoryCache;

        public PostViewComponent(IPostService postService, IMemoryCache memoryCache)
        {
            _postService = postService;
            _memoryCache = memoryCache;
        }

        public IViewComponentResult Invoke(int id = 0)
        {
            if (id == 0)
            {
                List<Post> cachedPosts;
                if (_memoryCache.TryGetValue("posts",out cachedPosts))
                {
                    return View(cachedPosts);
                }
                var posts = _postService.GetAllPosts();
                var orderedPosts = posts.OrderByDescending(o => o.Created).ToList();
                _memoryCache.Set("posts", orderedPosts,TimeSpan.FromDays(30));
                return View(orderedPosts);
            }

            var allPosts = _postService.GetByCount(id);
            return View(allPosts);
        }
    }
}
