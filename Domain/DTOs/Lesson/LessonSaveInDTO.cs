using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Lesson
{
    public class LessonSaveInDTO
    {
        public int? PkLesson { get; set; }
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? VideoUrl { get; set; }
        public string? Text { get; set; }
        public int TimeDuration { get; set; }
        public int Order { get; set; }
    }
}
