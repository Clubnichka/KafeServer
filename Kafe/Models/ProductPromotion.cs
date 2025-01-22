using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kafe.Models
{
    [PrimaryKey(nameof(ProductsId), nameof(PromotionsId))]
    public class ProductPromotion
    {
        public int ProductsId { get; set; }
        public int PromotionsId { get; set; }
        public Product Product { get; set; }=null;
        public Promotion Promotion { get; set; } = null;
    }
}
