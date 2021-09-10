using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private const string DataPath = @"C:\Users\usr\source\repos\C# DB\Entity Framework Core\08. JSON Processing\ProductShop\Datasets";

        private const string ResultPath = @"C:\Users\usr\source\repos\C# DB\Entity Framework Core\08. JSON Processing\ProductShop\Datasets\Result";

        public static void Main(string[] args)
        {
            var db = new ProductShopContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //Problem 02
            //string usersData = File.ReadAllText($"{DataPath}/users.json");
            //Console.WriteLine(ImportUsers(db, usersData));

            //Problem 03
            //string productsData = File.ReadAllText($"{DataPath}/products.json");
            //Console.WriteLine(ImportProducts(db, productsData));

            //Problem 04
            //string categoriesData = File.ReadAllText($"{DataPath}/categories.json");
            //Console.WriteLine(ImportCategories(db, categoriesData));

            //Problem 05
            //string categoriesProductsData = File.ReadAllText($"{DataPath}/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(db, categoriesProductsData));

            //Problem 06
            //var json = GetProductsInRange(db);
            //File.WriteAllText($"{ResultPath}/products-in-range.json", json);

            //Problem 07
            //var json = GetSoldProducts(db);
            //File.WriteAllText($"{ResultPath}/sold-products.json", json);

            //Problem 08
            //var json = GetCategoriesByProductsCount(db);
            //File.WriteAllText($"{ResultPath}/categories-by-products-count.json", json);

            //Problem 09
            var json = GetUsersWithProducts(db);
            File.WriteAllText($"{ResultPath}/users-products.json", json);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var users = JsonConvert.DeserializeObject<List<User>>(inputJson, serializerSettings);

            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                .Where(c => c.Name != null)
                .ToList();

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .OrderBy(p => p.Price)
                .ToList();

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(products, Formatting.Indented, serializerSettings);

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Where(u => u.ProductsSold.Count > 0)
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                    .Select(p => new
                    {
                        Name = p.Name,
                        Price = p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    })
                    .ToList()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(users, Formatting.Indented, serializerSettings);

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Select(cp => cp.Product).Count(),
                    AveragePrice = $"{c.CategoryProducts.Select(cp => cp.Product.Price).Average():f2}",
                    TotalRevenue = $"{c.CategoryProducts.Select(cp => cp.Product.Price).Sum():f2}"
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToList();

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(categories, Formatting.Indented, serializerSettings);

            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .Select(u => new
                {
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold.Count(),
                        Products = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            Name = p.Name,
                            Price = $"{p.Price:f2}"
                        })
                        .ToList()
                    }                
                })
                .ToList()
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToList();

            var result = new
            {
                UsersCount = users.Count,
                Users = users
            };

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented

            };

            var json = JsonConvert.SerializeObject(result, settings);

            return json;
        }
    }
}