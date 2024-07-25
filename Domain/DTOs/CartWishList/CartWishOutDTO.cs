using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.CartWishList
{
    public class CartWishOutDTO
    {
        public int PkCartWishList { get; set; }
        public int FkCourse { get; set; }
        public int FkUser { get; set; }
        public string Type { get; set; } = null!;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
