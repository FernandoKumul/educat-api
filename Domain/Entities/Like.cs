using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Like
    {
        [Key]
        public int PkLike { get; set; }
        
        [ForeignKey("User")]
        public int FkUser { get; set; }

        [ForeignKey("Comment")]
        public int FkComment { get; set; }
        public bool IsLike { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public virtual User User { get; set; } = null!;
        public virtual Comment Comment { get; set; } = null!;

    }
}
