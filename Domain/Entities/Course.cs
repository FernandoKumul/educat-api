using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course
    {
        [Key]
        public int PkCourse { get; set; }

        [ForeignKey("Instructor")]
        public int FKInstructor { get; set; }

        [ForeignKey("Category")]
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

        public virtual Category? Category { get; set; }
        public virtual Instructor Instructor { get; set; } = null!;
        public virtual ICollection<CartWishList> CartWishList { get; set; } = new List<CartWishList>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
    }
}
