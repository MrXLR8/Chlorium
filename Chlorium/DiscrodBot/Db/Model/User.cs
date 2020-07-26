using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiscrodBot
{
    public class User
    {
        [Key]
        public string id { get; set; }
        public string username { get; set; }
        public int points { get; set; } = 0;
    }
}
