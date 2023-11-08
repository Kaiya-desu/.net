using System;
using System.Collections.Generic;

namespace CSV.Models
{
    public class Uczelnia
    {
        public string CreatedAt { get; set; }
        public string Author{ get; set; }
        public HashSet<Student> Students { get; set; }
    }
}