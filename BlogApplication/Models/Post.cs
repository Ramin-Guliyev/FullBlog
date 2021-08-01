using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FullPost { get; set; }
        public string ImagePath { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
