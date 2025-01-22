using Microsoft.EntityFrameworkCore;

namespace Kafe.Models
{
    [PrimaryKey(nameof(SpecialOffersId), nameof(OrdersId))]
    public class OrderSpecialOfffer
    {
        public int Id { get; set; }
        public int SpecialOffersId { get; set; }
        public int OrdersId { get; set; }
        public Product Product { get; set; } = null;
        public Promotion Promotion { get; set; } = null;
    }
}
