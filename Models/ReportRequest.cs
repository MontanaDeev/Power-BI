using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBI.Models
{
    public class ReportRequest
    {
        public required string Name { get; set; }
        public int? CategoryId { get; set; }
        public string? PublicLink { get; set; } 
        public IFormFile? File { get; set; }
    }
}