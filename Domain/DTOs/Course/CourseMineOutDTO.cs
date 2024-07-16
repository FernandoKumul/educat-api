using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Course
{
    public class CourseMineOutDTO
    {
        public int PkCourse { get; set; }
        public int FKInstructor { get; set; }
        public int? FkCategory { get; set; }
        public string Title { get; set; } = null!;
        public decimal? Price { get; set; }
        public string? Cover { get; set; }
        public bool Active { get; set; } = false;

        public DateTime CretionDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
