using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Instructor
    {
        [Key]
        public int PkInstructor { get; set; }

        [ForeignKey("User")]
        public int FkUser { get; set; }
        public string? Occupation { get; set; } 
        public string? FacebookUser { get; set; }
        public string? YoutubeUser { get; set; }
        public string? LinkediId { get; set; }
        public string? TwitterUser { get; set; }
        public string? EmailPaypal { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();


    }
}
