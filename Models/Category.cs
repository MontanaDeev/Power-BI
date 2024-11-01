using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBI.Models
{
    public class Category
    {
        [Key]
        public int idCategory { get; set; }
        public string? Name { get; set; } = string.Empty;
        public List<Report>? Reports{ get; set; } = new List<Report>();
    }
}