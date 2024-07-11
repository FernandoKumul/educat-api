using Domain.DTOs.Course;
using Domain.DTOs.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.CartWishList
{
    public class CartItemOutDTO
    {
        public int PkCartWishList { get; set; }
        public int FkCourse { get; set; }
        public int FkUser { get; set; }
        public string Type { get; set; } = null!;
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public CourseMinOutDTO Course { get; set; } = null!;
        public UserMinOutDTO Instructor { get; set; } = null!;
    }
}
