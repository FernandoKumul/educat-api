using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Comment
{
    public class ReviewInDTO
    {
        public int courseId { get; set; }
        public string Text { get; set; } = null!;
        public int Score { get; set; }
    }
}
