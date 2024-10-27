using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBI.Models
{
    public class Report
    {
        [Key]
        public int idReport { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? publicLink { get; set; } = string.Empty;
        public int? idCategory { get; set; }
        public Category? Category { get; set; } = new Category();
        public string? FilePath { get; set; }
    }
}