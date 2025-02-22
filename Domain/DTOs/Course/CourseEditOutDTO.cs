﻿using Domain.DTOs.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Course
{
    public class CourseEditOutDTO
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
        public bool Active { get; set; } = false;
        public DateTime CretionDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public virtual ICollection<UnitEditOutDTO> Units { get; set; } = new List<UnitEditOutDTO>();
    }
}
