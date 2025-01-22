namespace Kafe.Models
{
    public class User
    {
        public User() {
            this.Orders = new HashSet<Order>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Level { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
