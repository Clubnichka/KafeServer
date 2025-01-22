using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kafe.Models;

namespace Kafe.Data;
public class KafeContext : DbContext
{
    public KafeContext(DbContextOptions<KafeContext> options) 
        : base(options)
    { }

    public KafeContext()
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<SpecialOffer> SpecialOffers { get; set; }
    //public DbSet<ProductPromotion> ProductPromotion { get; set; }
}