using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.PayPal
{
    public class GetTokenInDTO
    {
        public string Scope { get; set; } = null!;
        public string Access_token { get; set; } = null!;
        public string Token_type { get; set; } = null!;
        public string App_id { get; set; } = null!;
        public int Expires_in { get; set; }
        public string Nonce { get; set; } = null!;
    }
}
