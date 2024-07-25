using Domain.DTOs.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Unit
{
    public class UnitSaveInDTO
    {
        public int? PkUnit { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = null!;
        public ICollection<LessonSaveInDTO> Lessons { get; set; } = new List<LessonSaveInDTO>();
    }
}
