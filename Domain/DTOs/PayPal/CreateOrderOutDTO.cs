using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.PayPal
{
    public class CreateOrderOutDTO
    {
        public string intent { get; set; } = "CAPTURE";
        public ICollection<PurchaseUnits> purchase_units { get; set; } = new List<PurchaseUnits>();
    }

    public class PurchaseUnits
    {
        public string? reference_id { get; set; }
        public AmountCreateOrder amount { get; set; } = null!;

    }

    public class AmountCreateOrder
    {
        public string currency_code { get; set; } = "MXN";
        public string value { get; set; } = null!;
    }
}
