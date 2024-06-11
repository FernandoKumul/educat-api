using Domain.DTOs.Lesson;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Unit
{
    public class UnitEditOutDTO
    {
        public int PkUnit { get; set; }
        public int FkCourse { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<LessonEditOutDTO> Lessons { get; set; } = new List<LessonEditOutDTO>();
    }
}
