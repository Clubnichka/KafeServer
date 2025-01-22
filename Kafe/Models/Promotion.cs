namespace Kafe.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsActive { get; set; }
        public List<Product> Products { get; set; } = [];
    }
}
