using Kafe.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Kafe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Kafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly KafeContext db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, KafeContext context)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet("getCurrentOrder")]
        public IActionResult getCurrentOrder([FromQuery] int userid)
        {
            Order order = db.Orders.FirstOrDefault(o=>o.UserId == userid&&o.Status!=4);
            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return Ok(new Order { Status = 0 });
            }
        }

        [HttpGet]
        [Route("getProducts")]
        public IActionResult getProducts()
        {
            var products = db.Products.Where(p => p.IsActive == 1);

            return Ok(products);
        }

        [HttpGet("getPromotions")]
        public IActionResult getPromotions()
        {
            List<Promotion> promotions = new List<Promotion>();
            int i = 0;

            foreach (Promotion promo in db.Promotions.Include(p => p.Products))
            {
                List<Product> products=new List<Product>();
                foreach (Product p in promo.Products)
                {
                    products.Add(new Product { Id=p.Id, Name=p.Name,Description=p.Description,Price=p.Price});
                }
                promotions.Add(new Promotion { Id=promo.Id, Name=promo.Name, Description=promo.Description,Products=products });
                
            }
            return Ok(promotions);
        }

        [HttpGet("del")]
        public IActionResult Del()
        {
            db.Orders.ExecuteDelete();
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetData()
        {
            var data = db.Users.First();
            return Ok(data);
        }

        [HttpGet("getUserOrders")]
        public IActionResult getUserOrders([FromQuery] int userid)
        {
            var orders = db.Orders
                .Where(o=>o.UserId==userid&&o.Status==4);
            return Ok(orders);
        }

        [HttpGet("getProductsByOrderId")]
        public IActionResult getProductsByOrderId([FromQuery]int orderid)
        {
            var order = db.Orders.Include(o => o.Products).Where(o => o.Id.Equals(orderid)).FirstOrDefault();
            if (order == null)
            {
                return NotFound(); // Обработка случая, когда заказ не найден
            }

            List<Product> products = new List<Product>();

            // Группируем продукты по Id и добавляем их в результат с учетом количества
            var groupedProducts = order.Products.GroupBy(p => p.Id);

            foreach (var group in groupedProducts)
            {
                var product = group.First(); // Берем первый продукт из группы
                int count = group.Count(); // Количество вхождений

                // Добавляем продукт столько раз, сколько он встречается
                for (int i = 0; i < count; i++)
                {
                    products.Add(new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Description = product.Description
                    });
                }
            }

            return Ok(products);
        }

        [HttpGet("getOffersByOrderId")]
        public IActionResult getOffersByOrderId([FromQuery] int orderid)
        {
            var order = db.Orders.Include(o => o.SpecialOffers)
                .Where(o => o.Id.Equals(orderid)).FirstOrDefault();
            var offers = db.SpecialOffers.Include(o => o.Products).Where(o => order.SpecialOffers.Contains(o));
            int i = 0;
            int size = order.SpecialOffers.Count();
            SpecialOffer[] result = new SpecialOffer[size];
            foreach (SpecialOffer offer in offers)
            {
                if (i < size)
                {
                    result[i] = new SpecialOffer { Id = offer.Id, Name = offer.Name, Description = offer.Description, Price = offer.Price };
                    foreach (Product p in offer.Products)
                    {
                        result[i].Products.Add(new Product { Id = p.Id, Name = p.Name, Price = p.Price, Description = p.Description });
                    }
                }
                else
                {
                    break;
                }
                i += 1;
            }
            return Ok(result);
        }

        [HttpGet("getSpecialOffers")]
        public IActionResult getSpecialOffers([FromQuery] int userid)
        {
            User user = db.Users.First(p => p.Id == userid);
            List < SpecialOffer > offers= db.SpecialOffers.Include(o=>o.Products).Where(o=>o.IsActive==1).ToList();
            int level = user.Level;
            int i = 0;
            SpecialOffer[] result=new SpecialOffer[level];
            foreach(SpecialOffer offer in offers)
            {
                if (i < level)
                {
                    result[i] = new SpecialOffer {Id=offer.Id, Name=offer.Name, Description=offer.Description, Price=offer.Price };
                    foreach(Product p in offer.Products)
                    {
                        result[i].Products.Add(new Product { Id = p.Id, Name = p.Name, Price = p.Price, Description = p.Description });
                    }
                }
                else
                {
                    break;
                }
                i += 1;
            }
            return Ok(result);
        }
        [HttpGet("getSpecialOffersByIds")]
        public IActionResult getSpecialOffersByIds([FromQuery] List<int> ids)
            {
            List<SpecialOffer> offers=db.SpecialOffers.Include(o=> o.Products).Where(o=>ids.Contains(o.Id)).ToList();
            int i = 0;
            SpecialOffer[] result = new SpecialOffer[ids.Count()];
            foreach (SpecialOffer offer in offers)
            {
                if (i < ids.Count())
                {
                    result[i] = new SpecialOffer { Id = offer.Id, Name = offer.Name, Description = offer.Description, Price = offer.Price };
                    foreach (Product p in offer.Products)
                    {
                        result[i].Products.Add(new Product { Id = p.Id, Name = p.Name, Price = p.Price, Description = p.Description });
                    }
                }
                else
                {
                    break;
                }
                i += 1;
            }
            return Ok(result);
        }

        [HttpGet("getLevel")]
        public IActionResult getLevel([FromQuery] int userid)
        {
            User user = db.Users.First(o=>o.Id == userid);
            int level = user.Level;
            return Ok(level);
        }

        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
            public string Description { get; set; }
        }

        [HttpPost("addOrder")]
        public IActionResult addOrder([FromQuery] List<int> ids, [FromQuery] List<int> ids2, [FromQuery] int userid)
        {
            var productGroups = ids.GroupBy(id => id)
                       .Select(group => new { Id = group.Key, Count = group.Count() })
                       .ToList();

            // Создаем список продуктов
            var products = new List<Product>();

            // Получаем продукты из базы данных и добавляем их в список нужное количество раз
            foreach (var group in productGroups)
            {
                var product = db.Products.FirstOrDefault(p => p.Id == group.Id);
                if (product != null)
                {
                    for (int i = 0; i < group.Count; i++)
                    {
                        products.Add(product);
                    }
                }
            }
            var offers = db.SpecialOffers.Where(o => ids2.Contains(o.Id)).ToList();
            Order order = new Order { Products = products, SpecialOffers = offers, UserId = userid, Status = 1 };
            db.Orders.Add(order);
            db.SaveChanges();
            User u = db.Users.Include(u => u.Orders).First(u => u.Id == userid);
            int ordernumber = u.Orders.Count();
            int level = 1;
            if (ordernumber >= 2 && ordernumber < 10)
            {
                level = 2;
            }
            else if (ordernumber >= 10 && ordernumber < 15)
            {
                level = 3;
            }
            else if (ordernumber >= 15)
            {
                level = 4;
            }
            u.Level = level;
            db.Update(u);
            db.SaveChanges();
            return Ok("Success");
        }

        [HttpPost("addOrder1")]
        public IActionResult addOrder([FromQuery] List<int> ids, [FromQuery] int userid, [FromQuery] int type)
        {
            if (type == 1)
            {
                var productGroups = ids.GroupBy(id => id)
                        .Select(group => new { Id = group.Key, Count = group.Count() })
                        .ToList();

                // Создаем список продуктов
                var products = new List<Product>();

                // Получаем продукты из базы данных и добавляем их в список нужное количество раз
                foreach (var group in productGroups)
                {
                    var product = db.Products.FirstOrDefault(p => p.Id == group.Id);
                    if (product != null)
                    {
                        for (int i = 0; i < group.Count; i++)
                        {
                            products.Add(product);
                        }
                    }
                }
                Order order = new Order { Products = products, UserId = userid, Status = 1 };
                db.Orders.Add(order);
                db.SaveChanges();
            }
            else if (type == 2)
            {
                var offers = db.SpecialOffers.Where(o => ids.Contains(o.Id)).ToList();
                Order order = new Order { SpecialOffers = offers, UserId = userid, Status = 1 };
                db.Orders.Add(order);
                db.SaveChanges();
            }
            User u = db.Users.Include(u => u.Orders).First(u => u.Id == userid);
            int ordernumber = u.Orders.Count();
            int level = 1;
            if (ordernumber >= 2 && ordernumber < 10)
            {
                level = 2;
            }
            else if (ordernumber >= 10 && ordernumber < 15)
            {
                level = 3;
            }
            else if (ordernumber >= 15)
            {
                level = 4;
            }
            u.Level = level;
            db.Update(u);
            db.SaveChanges();
            return Ok("Success");
        }

        private void UpdateUserLevel(int userid)
        {
            User u = db.Users.Include(u => u.Orders).First(u => u.Id == userid);
            int ordernumber = u.Orders.Count();
            int level = 1;

            if (ordernumber >= 2 && ordernumber < 10)
            {
                level = 2;
            }
            else if (ordernumber >= 10 && ordernumber < 15)
            {
                level = 3;
            }
            else if (ordernumber >= 15)
            {
                level = 4;
            }

            u.Level = level;
            db.Update(u);
            db.SaveChanges();
        }

        //[HttpPost("addOrder1")]
        //public IActionResult addOrder([FromQuery] List<int> ids, [FromQuery] int userid)
        //{
        //    var products = db.Products.Where(p => ids.Contains(p.Id)).ToList();
        //    Order order = new Order { Products = products, UserId = userid };
        //    db.Orders.Add(order);
        //    db.SaveChanges();
        //    return Ok("Success");
        //}
        [HttpGet("getProductsByIds")]
        public IActionResult GetProductsByIds([FromQuery] List<int> ids)
        {
            // Получаем продукты по id
            var products = db.Products.Where(p => ids.Contains(p.Id)).ToList();

            // Создаем список для хранения результатов
            var result = new List<Product>();

            // Группируем идентификаторы и добавляем продукты в результат в соответствии с количеством их вхождений
            foreach (var group in ids.GroupBy(id => id))
            {
                var product = products.FirstOrDefault(p => p.Id == group.Key);
                if (product != null)
                {
                    // Добавляем продукт столько раз, сколько раз он встречается в ids
                    result.AddRange(Enumerable.Repeat(product, group.Count()));
                }
            }

            return Ok(result);
        }

        //[HttpGet("enter")]
        //public IActionResult enter([FromQuery] string name, [FromQuery] string password)
        //{
        //    List<User> users = db.Users.ToList();

        //    foreach (User u in users)
        //    {
        //        Console.WriteLine(u.Name + " " + u.Password);
        //        if (u.Name.Equals(name)&&u.Password.Equals(password))
        //        {
        //            return Ok(u);
        //            break;
        //        }
        //    }
        //    return Ok(new User() { Name="nea"});
        //}

        [HttpGet("enter")]
        public IActionResult enter([FromQuery] string name, [FromQuery] string password)
        {
            Console.WriteLine($"Received Name: '{name}', Password: '{password}'");

            List<User> users = db.Users.ToList();

            foreach (User u in users)
            {
                Console.WriteLine($"Comparing User: '{u.Name}' with '{name}'");
                Console.WriteLine($"Comparing Password: '{u.Password}' with '{password}'");

                if (u.Name.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase) &&
    u.Password.Trim().Equals(password.Trim()))
                {
                    return Ok(u); // Пользователь найден
                }
            }

            // Если пользователь не найден
            return NotFound(new { Message = "User not found" });
        }

        [HttpGet("register")]
        public IActionResult register([FromQuery] string name, [FromQuery] string password)
        {
            bool add = true;
            foreach (User u in db.Users)
            {
                if (u.Name == name)
                {
                    return Ok(new User() );
                    add= false; 
                }
            }
            if (add)
            {
                User user = new User();
                user.Name = name;
                user.Password = password;
                user.Level = 1;
                db.Users.Add(user);
                db.SaveChanges();
                user=db.Users.FirstOrDefault(u=>u.Name==name);
                return Ok(user);
            }
            return Ok();
        }
        public IActionResult DO()
        {
            User user1 = new User { Name = "Tom", Password="11", Level=1 };
            User user2 = new User { Name = "Sam", Password = "11", Level = 1 };
            db.Users.Add(user1);
            db.Users.Add(user2);
            db.SaveChanges();
            return View("~/Views/Home/Index.cshtml");
        }

        public async Task<IActionResult> DO1()
        {
            User user1 = db.Users.First();
            User user2 = db.Users.Find(2);
            List<User> Userset= new List<User>();
            Userset.Add(user1);
            Userset.Add(user2);
            return View("~/Views/Home/Data.cshtml", Userset);
        }

        public async Task<IActionResult> DO2()
        {
            Product product = new Product { Name = "Pizza", Description = "Tasty Shit Number 2", Price = 50, IsActive = 1 };
            db.Products.Add(product);
            db.SaveChanges();
            return View("~/Views/Home/Index.cshtml");
        }

        public async Task<IActionResult> DO3()
        {
            Promotion prom = new Promotion { Name = "Discount 0", Description = "Useless", IsActive = 1 };
            prom.Products.Add(db.Products.First());
            db.Promotions.Add(prom);
            db.SaveChanges();
            return View("~/Views/Home/Index.cshtml");
        }

        public async Task<IActionResult> DO4()
        {
            SpecialOffer spec = new SpecialOffer
            {
                Name = "Cheaper Burger",
                Description = "Same shit but cheaper"
                ,
                Price = 25,
                IsActive = 1,
            };
            spec.Products.Add(db.Products.First());
            db.SpecialOffers.Add(spec);
            db.SaveChanges();
            return View("~/Views/Home/Index.cshtml");
        }

        public async Task<IActionResult> DO5()
        {
            Order order = new Order();
            order.User = db.Users.Find(4);
            order.Products.Add(db.Products.First(x=>x.Name=="Pizza"));
            order.SpecialOffers.Add(db.SpecialOffers.First());
            db.Orders.Add(order);
            db.SaveChanges();
            List<Order> orders = new List<Order>();
            orders.Add(order);
            return View("~/Views/Home/Order.cshtml", orders);
        }

        public async Task<IActionResult> DO6()
        {
           var promo = db.Promotions.Include(o=>o.Products).First(x=>x.Id==1007);
           return View("~/Views/Home/Promo.cshtml",promo);
        }

        [Route("DO7")]
        public async Task<IActionResult> DO7()
        {
            var orders = db.Orders
        .Include(o => o.User)
        .Include(o => o.Products)
        .Include(o => o.SpecialOffers)
        .ToList();

            return View("~/Views/Home/Order.cshtml", orders);
        }

        public async  Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
