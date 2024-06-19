using Domain.DTOs.Unit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Course
{
    public class CourseSaveInDTO
    {
        public int? FkCategory { get; set; }
        public string Title { get; set; } = null!;
        public string? Summary { get; set; }
        public string? Language { get; set; }
        public string? Difficulty { get; set; }
        [Range(0, 99999)]
        public decimal? Price { get; set; }
        public string? VideoPresentation { get; set; }
        public string? Cover { get; set; }
        public string? Requeriments { get; set; }
        public string? Description { get; set; }
        public string? LearnText { get; set; }
        public string? Tags { get; set; }

        public ICollection<UnitSaveInDTO> Units { get; set; } = new List<UnitSaveInDTO>();
    }
}
