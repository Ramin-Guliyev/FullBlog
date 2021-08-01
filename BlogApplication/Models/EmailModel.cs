using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
