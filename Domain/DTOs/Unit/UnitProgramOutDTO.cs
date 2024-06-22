using Domain.DTOs.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Unit
{
    public class UnitProgramOutDTO
    {
        public int PkUnit { get; set; }
        public int FkCourse { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = null!;

        public ICollection<LessonProgramOutDTO> Lessons { get; set; } = new List<LessonProgramOutDTO>();
    }
}
