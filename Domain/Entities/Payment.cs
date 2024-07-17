using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment
    {
        [Key]
        public int PkPayment { get; set; }
        [ForeignKey("Course")]
        public int? FkCourse { get; set; }

        [ForeignKey("User")]
        public int FkUser { get; set; }
        public string OrderId { get; set; } = null!;
        public string? PayerId { get; set; }
        public string PayerEmail { get; set;} = null!;
        public decimal PaymentAmount { get; set;}
        public string Currency { get; set;} = null!;
        public string PaymentStatus { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public string? CardType { get; set; } = null!;
        public bool Archived { get; set; } = false;
        public DateTime TransactionDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;


        public virtual Course? Course { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public ICollection<LessonProgress> LessonsProgress { get; set; } = new List<LessonProgress>();

    }
}
