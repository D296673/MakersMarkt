using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakersMarkt.Data
{
    class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public User Buyer { get; set; }
        public User Seller { get; set; }
        public Product Product { get; set; }

    }
}
