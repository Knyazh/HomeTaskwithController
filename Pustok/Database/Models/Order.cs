using Pustok.Contracts;
using Pustok.Database.Base;
using Pustok.Database.Models;

namespace Pustok.Database.Models
{
    public class Order : BaseEntity<int>, IAuditable
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public string TracingCode { get; set; }
        public StatusOrderItem.OrderItemStatusValue OrderItemStatusValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
