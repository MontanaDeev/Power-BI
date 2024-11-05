using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBI.Models
{
    public class User
    {
        [Key]
        public int idUser { get; set; }
        public string? name { get; set; }
        public string lastname { get; set; }
        public string nickname { get; set; }
        public string password { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}