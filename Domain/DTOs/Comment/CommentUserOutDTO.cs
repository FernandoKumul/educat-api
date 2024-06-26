using Domain.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Comment
{
    public class CommentUserOutDTO
    {
        public int PkComment { get; set; }
        public int? FkCourse { get; set; }
        public int? FkLesson { get; set; }
        public string Text { get; set; } = null!;
        public decimal? Score { get; set; }
        public DateTime CretionDate { get; set; } = DateTime.Now;
        public UserMinOutDTO User { get; set; } = null!;
    }
}
