using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment
    {
        [Key]
        public int PkComment { get; set; }
        
        [ForeignKey("User")]
        public int FkUser { get; set; }

        [ForeignKey("Course")]
        public int? FkCourse { get; set; }
        
        [ForeignKey("Lesson")]
        public int? FkLesson { get; set; }
        public string Text { get; set; } = null!;
        public decimal Score { get; set; }
        public DateTime CretionDate { get; set; } = DateTime.Now;


        public virtual User User { get; set; } = null!;
        public virtual Course? Course { get; set; }
        public virtual Lesson? Lesson { get; set; }
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
