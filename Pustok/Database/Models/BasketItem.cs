using Pustok.Database.Base;

namespace Pustok.Database.Models;

public class BasketItem : BaseEntity<int>, IAuditable
{
    public int BasketId { get; set; }
    public Basket Basket { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int ColorId { get; set; }
    public Color Color { get; set; }

    public int SizeId { get; set; }
    public Size Size { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsOrdered { get; set; }
}
