using Microsoft.EntityFrameworkCore;

namespace Kafe.Models
{
    [PrimaryKey(nameof(OrdersId))]
    public class ProductOrder
    {
        public int Id { get; set; }
        public int ProductsId { get; set; }
        public int OrdersId { get; set; }
        public Product Product { get; set; } = null;
        public Promotion Promotion { get; set; } = null;
    }
}
