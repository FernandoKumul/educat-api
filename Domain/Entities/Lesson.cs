using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Lesson
    {
        [Key]
        public int PkLesson { get; set; }
        
        [ForeignKey("Unit")]
        public int Fkunit { get; set; }
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? VideoUrl { get; set; }
        public string? Text { get; set; }
        public int TimeDuration { get; set; }
        public int Order { get; set; }
        public DateTime CretionDate { get; set; } = DateTime.Now;

        public virtual Unit Unit { get; set; } = null!;
        public virtual ICollection<LessonProgress> LessonsProgress { get; set; } = new List<LessonProgress>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();


    }
}
