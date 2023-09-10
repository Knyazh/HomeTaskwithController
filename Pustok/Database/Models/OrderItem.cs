using Pustok.Database.Base;
using Pustok.Database.Models;

namespace Pustok.Database.Models
{
    public class OrderItem : BaseEntity<int>, IAuditable
    {
        public BasketItem BasketItem { get; set; }
        public string OrderSizes { get; set; }
        public decimal OrderPrice { get; set; }
        public decimal? OrderQuantity { get; set; }
        public string OrderName { get; set; }
        public string OrderColor { get; set; }
        public string OrderPhoto { get; set; }
        public string OrderDescription { get; set; }
        public string OrderCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public Order Order { get; set; }
        public int OrderId { get; set; }
    }
}
