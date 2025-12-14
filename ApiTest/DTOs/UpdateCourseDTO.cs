using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.DTOs
{
    public class UpdateCourseDTO
    {
        public int Id { get; set; }
        [Range(1,4)]
        public int Year { get; set; }
    }
}