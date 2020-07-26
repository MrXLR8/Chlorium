using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiscrodBot
{
    public class GuessedWord
    {
        [Key]
        public Guid id { get; set; }
        public User user { get; set; }
        public string word { get; set; }
        public bool active { get; set; } = false;
        public DateTime activated { get; set; }
        public DateTime deactivated { get; set; }
    }
}
