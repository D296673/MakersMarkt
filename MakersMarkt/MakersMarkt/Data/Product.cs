using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.DateTimeFormatting;

namespace MakersMarkt.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public string Material { get; set; }
        public DateTime ProductionTime { get; set; }
        public string Complexity { get; set; }
        public int Durability { get; set; }
        public string UniqueFeatures { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public int MakerId { get; set; }
        public User Maker { get; set; }
        public Type Type { get; set; }
    }
}
