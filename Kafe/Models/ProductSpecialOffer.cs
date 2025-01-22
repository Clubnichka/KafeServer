using Microsoft.EntityFrameworkCore;

namespace Kafe.Models
{
    [PrimaryKey(nameof(ProductsId), nameof(SpecialOffersId))]
    public class ProductSpecialOffer
    {
        public int ProductsId { get; set; }
        public int SpecialOffersId { get; set; }
        public Product Product { get; set; } = null;
        public Promotion Promotion { get; set; } = null;
    }
}
