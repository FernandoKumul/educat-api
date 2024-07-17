using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Lesson
{
    public class LessonOutDTO
    {
        public int PkLesson { get; set; }
        public int Fkunit { get; set; }
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? VideoUrl { get; set; }
        public string? Text { get; set; }
        public int TimeDuration { get; set; }
        public int Order { get; set; }
        public DateTime CretionDate { get; set; } = DateTime.Now;
    }
}
