using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Lesson
{
    public class LessonProgramOutDTO
    {
        public int PkLesson { get; set; }
        public int Fkunit { get; set; }
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int TimeDuration { get; set; }
        public bool Completed { get; set; } = false;
        public int Order { get; set; }
        public DateTime CretionDate { get; set; } = DateTime.Now;
    }
}
