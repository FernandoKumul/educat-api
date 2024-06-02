using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Category
{
    public class CategoryOutDTO
    {
        public int PkCategory { get; set; }
        public string Name { get; set; } = null!;
    }
}
