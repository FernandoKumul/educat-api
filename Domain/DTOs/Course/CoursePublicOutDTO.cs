using Domain.DTOs.Unit;
using Domain.DTOs.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Course
{
    public class CoursePublicOutDTO
    {
        public int PkCourse { get; set; }
        public int FKInstructor { get; set; }
        public int? FkCategory { get; set; }
        public string Title { get; set; } = null!;
        public string? Summary { get; set; }
        public string? Language { get; set; }
        public string? Difficulty { get; set; }
        public decimal? Price { get; set; }
        public string? VideoPresentation { get; set; }
        public string? Cover { get; set; }
        public string? Requeriments { get; set; }
        public string? Description { get; set; }
        public string? LearnText { get; set; }
        public string? Tags { get; set; }
        public decimal Rating { get; set; }
        public int NumberStudents { get; set; }
        public bool Purchased { get; set; } = false;
        public bool Active { get; set; } = false;
        
        public DateTime CretionDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public UserMinOutDTO Instructor { get; set; } = null!;
        public ICollection<UnitProgramOutDTO> Units { get; set; } = new List<UnitProgramOutDTO>();

    }
}
