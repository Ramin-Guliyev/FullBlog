using BlogApplication.Models;
using BlogApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace BlogApplication.Controllers
{
    public class RaminController : Controller
    {
        private IPostService _postService;
        private UserManager<IdentityUser> _userManager; 
        private SignInManager<IdentityUser> _signInManager;
        private IMessageService _messageService;
        private IEmailService _emailService;
        private IConfiguration _configuration;
        private IMemoryCache _memoryCache;

        public RaminController(IPostService postService,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IMessageService messageService, IEmailService emailService,
            IConfiguration configuration, IMemoryCache memoryCache)
        {
            _postService = postService;
            _signInManager = signInManager;
            _userManager = userManager;
            _messageService = messageService;
            _emailService = emailService;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto postDto)
        {
            if (ModelState.IsValid)
            {
                await _postService.AddAsync(postDto);
                _memoryCache.Remove("posts");
                return RedirectToAction("Main");
            }
            return View();
        }
        public FileResult DownloadFile()
        {
            byte[] bytes = _postService.DownloadFile();
            return File(bytes, "application/octet-stream", "Ramin Guliyev.pdf");
        }

        [Authorize(Roles = "Admin")]
        [Route("upload_ckeditor")]
        [HttpPost]
        public async Task< IActionResult> UploadCKEditor(IFormFile upload)
        {
            if (upload == null)
                return View("Error");
            
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            await  _postService.CkEditorPost(upload, fileName);
            var random = new Random();

            return new JsonResult(new { uploaded = random.Next(10000), fileName = upload.FileName, url = "/uploads/" + fileName });
        }

        [HttpPost]
        public async Task< IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return View(loginModel);
            
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
                return View("Index",loginModel);
            
            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password,false, false);
            if (result.Succeeded)
                return RedirectToAction("Main");

            return View("Error");
            
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Main()
        {
            var posts = _postService.GetAllPosts();

            return View(posts);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.DeleteAsync(id);
            _memoryCache.Remove("posts");

            return RedirectToAction("Main");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdatePost(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return View("Error");
            
            return View(post);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Post post)
        {
            if (ModelState.IsValid)
            {
                await _postService.UpdateAsync(post);
                _memoryCache.Remove("posts");
                return RedirectToAction("Main");
            }
            return View("Error");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SendEmail(int id)
        {
            var email = _emailService.GetAllEmails();
            var post = await _postService.GetByIdAsync(id);
            string htmlMessage = $"<center><h1>Hi my friend</h1><h2>Do you want to read about {post.Title}</h2> <br/> <a href='{_configuration["AppUrl"]}post/PostDetail/{post.Id}'><h2>Click here and see full post</h2></a></center>";
            foreach (var item in email)
            {
                await _messageService.EmailSenderAsync(item.EmailAddress,"Ramin - Blog New Post is available", htmlMessage);
            }

            return RedirectToAction("Main");
        }
    }
}
