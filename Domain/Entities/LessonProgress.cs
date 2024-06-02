using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LessonProgress
    {
        [Key]
        public int PkLessonProgress { get; set; }
        [ForeignKey("Lesson")]
        public int FkLesson { get; set; }
        [ForeignKey("Payment")]
        public int FkPayment { get; set; }
        public bool Done { get; set; } = false;

        public virtual Lesson Lesson { get; set; } = null!;
        public virtual Payment Payment { get; set; } = null!;
    }
}
