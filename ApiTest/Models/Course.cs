using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Models
{
    public class Course : BaseEntity
    {
        [Range(1,4)]
        public int Year { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}