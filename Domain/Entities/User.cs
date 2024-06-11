using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        public int PkUser { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string? Description { get; set; }
        public bool IsInstructor { get; set; }
        public bool ValidatedEmail { get; set; } = false;
        public DateTime CreationDate { get; set; }


        public virtual ICollection<CartWishList> CartWishLists { get; set; } = new List<CartWishList>();
        public virtual Instructor? Instructor { get; set; }
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}
