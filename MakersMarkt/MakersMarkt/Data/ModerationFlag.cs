using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakersMarkt.Data
{
    class ModerationFlag
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int? ReviewId { get; set; }
        public int ModeratorId { get; set; }
        public string Reason { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        public Review Review { get; set; }
        public User Moderator { get; set; }
    }
}
