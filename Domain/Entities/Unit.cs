using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Unit
    {
        [Key]
        public int PkUnit { get; set; }

        [ForeignKey("Course")]
        public int FkCourse { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = null!;

        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();


    }
}
