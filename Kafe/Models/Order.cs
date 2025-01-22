using System.ComponentModel.DataAnnotations.Schema;

namespace Kafe.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        public List<Product> Products { get; set; } = [];
        public List<SpecialOffer> SpecialOffers { get; set; } = [];
    }
}
