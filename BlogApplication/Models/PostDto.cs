using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    public class PostDto
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        public string FullPost { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile FormFile { get; set; }
    }
}
