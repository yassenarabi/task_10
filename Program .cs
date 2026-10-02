using Microsoft.EntityFrameworkCore;

// ------------------------------------------------------------
// Scaffold command (run once in Package Manager Console / terminal):
//
// dotnet ef dbcontext scaffold "Server=.;Database=BikeStores;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --context BikeStoresContext --output-dir Models
//
// Names used below are the default scaffold names:
// DbSets: Customers, Orders, OrderItems, Products, Brands, Categories, Staffs, Stores, Stocks
// ------------------------------------------------------------

using var db = new BikeStoresContext();

// Parameters (change as you like)
int staffId = 3;
string categoryName = "Mountain Bikes";
short modelYear = 2018;
int categoryId = 6;
int productId = 1;

static void Title(string t) => Console.WriteLine($"\n===== {t} =====");

// 1. Customers' first name, last name, email
Title("1");
var q1 = db.Customers
    .Select(c => new { c.FirstName, c.LastName, c.Email })
    .ToList();
q1.ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName} - {x.Email}"));

// 2. Orders processed by a specific staff member
Title("2");
var q2 = db.Orders.Where(o => o.StaffId == staffId).ToList();
q2.ForEach(o => Console.WriteLine($"Order {o.OrderId} | {o.OrderDate}"));

// 3. Products in category "Mountain Bikes"
Title("3");
var q3 = db.Products
    .Where(p => p.Category.CategoryName == categoryName)
    .Select(p => new { p.ProductId, p.ProductName })
    .ToList();
q3.ForEach(p => Console.WriteLine($"{p.ProductId} - {p.ProductName}"));

// 4. Total orders per store
Title("4");
var q4 = db.Orders
    .GroupBy(o => new { o.StoreId, o.Store.StoreName })
    .Select(g => new { g.Key.StoreId, g.Key.StoreName, OrdersCount = g.Count() })
    .ToList();
q4.ForEach(x => Console.WriteLine($"{x.StoreName} (#{x.StoreId}): {x.OrdersCount}"));

// 5. Orders not shipped yet
Title("5");
var q5 = db.Orders.Where(o => o.ShippedDate == null).ToList();
q5.ForEach(o => Console.WriteLine($"Order {o.OrderId} | Status {o.OrderStatus}"));

// 6. Customer full name + number of orders
Title("6");
var q6 = db.Customers
    .Select(c => new
    {
        FullName = c.FirstName + " " + c.LastName,
        OrdersCount = c.Orders.Count()
    })
    .ToList();
q6.ForEach(x => Console.WriteLine($"{x.FullName}: {x.OrdersCount}"));

// 7. Products never ordered
Title("7");
var q7 = db.Products
    .Where(p => !p.OrderItems.Any())
    .Select(p => new { p.ProductId, p.ProductName })
    .ToList();
q7.ForEach(p => Console.WriteLine($"{p.ProductId} - {p.ProductName}"));

// 8. Products with stock quantity < 5 in any store
Title("8");
var q8 = db.Products
    .Where(p => p.Stocks.Any(s => s.Quantity < 5))
    .Select(p => new { p.ProductId, p.ProductName })
    .ToList();
q8.ForEach(p => Console.WriteLine($"{p.ProductId} - {p.ProductName}"));

// 9. First product
Title("9");
var q9 = db.Products.OrderBy(p => p.ProductId).First();
Console.WriteLine($"{q9.ProductId} - {q9.ProductName}");

// 10. Products with a certain model year
Title("10");
var q10 = db.Products.Where(p => p.ModelYear == modelYear).ToList();
q10.ForEach(p => Console.WriteLine($"{p.ProductName} ({p.ModelYear})"));

// 11. Each product with the number of times it was ordered
Title("11");
var q11 = db.Products
    .Select(p => new { p.ProductName, TimesOrdered = p.OrderItems.Count() })
    .ToList();
q11.ForEach(x => Console.WriteLine($"{x.ProductName}: {x.TimesOrdered}"));

// 12. Number of products in a specific category
Title("12");
int q12 = db.Products.Count(p => p.CategoryId == categoryId);
Console.WriteLine($"Products in category {categoryId}: {q12}");

// 13. Average list price
Title("13");
decimal q13 = db.Products.Average(p => p.ListPrice);
Console.WriteLine($"Average list price: {q13:F2}");

// 14. Product by ID
Title("14");
var q14 = db.Products.FirstOrDefault(p => p.ProductId == productId);
Console.WriteLine(q14 == null ? "Not found" : $"{q14.ProductId} - {q14.ProductName} - {q14.ListPrice}");

// 15. Products ordered with quantity > 3 in any order
Title("15");
var q15 = db.Products
    .Where(p => p.OrderItems.Any(oi => oi.Quantity > 3))
    .Select(p => new { p.ProductId, p.ProductName })
    .ToList();
q15.ForEach(p => Console.WriteLine($"{p.ProductId} - {p.ProductName}"));

// 16. Each staff member + number of orders processed
Title("16");
var q16 = db.Staffs
    .Select(s => new
    {
        FullName = s.FirstName + " " + s.LastName,
        OrdersCount = s.Orders.Count()
    })
    .ToList();
q16.ForEach(x => Console.WriteLine($"{x.FullName}: {x.OrdersCount}"));

// 17. Active staff + phone numbers (active is tinyint: 1 = true)
Title("17");
var q17 = db.Staffs
    .Where(s => s.Active == 1)
    .Select(s => new { s.FirstName, s.LastName, s.Phone })
    .ToList();
q17.ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName} - {x.Phone}"));

// 18. Products with brand name and category name
Title("18");
var q18 = db.Products
    .Select(p => new
    {
        p.ProductName,
        p.Brand.BrandName,
        p.Category.CategoryName
    })
    .ToList();
q18.ForEach(x => Console.WriteLine($"{x.ProductName} | {x.BrandName} | {x.CategoryName}"));

// 19. Completed orders (order_status = 4)
Title("19");
var q19 = db.Orders.Where(o => o.OrderStatus == 4).ToList();
q19.ForEach(o => Console.WriteLine($"Order {o.OrderId} | {o.OrderDate}"));

// 20. Each product with total quantity sold
Title("20");
var q20 = db.Products
    .Select(p => new
    {
        p.ProductName,
        TotalSold = p.OrderItems.Sum(oi => (int?)oi.Quantity) ?? 0
    })
    .ToList();
q20.ForEach(x => Console.WriteLine($"{x.ProductName}: {x.TotalSold}"));
