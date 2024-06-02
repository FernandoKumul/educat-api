using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartWishList
    {
        [Key]
        public int PkCartWishList { get; set; }

        [ForeignKey("Course")]
        public int FkCourse { get; set; }

        [ForeignKey("User")]
        public int FkUser { get; set; }

        public string Type { get; set; } = null!;
        public DateTime CreationDate { get; set; } = DateTime.Now;


        public virtual Course Course { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
