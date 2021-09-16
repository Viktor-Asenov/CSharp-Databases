using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private const string DATASETS_DIRECTORY_PATH = @"./Datasets";

        private const string RESULTS_DIRECTORY_PATH = @"./Datasets/Results";

        public static void Main(string[] args)
        {
            var db = new ProductShopContext();

            //Problem 01
            //var users = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/users.xml");
            //Console.WriteLine(ImportUsers(db, users));

            //Problem 02
            //var products = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/products.xml");
            //Console.WriteLine(ImportProducts(db, products));

            //Problem 03
            //var categories = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/categories.xml");
            //Console.WriteLine(ImportCategories(db, categories));

            //Problem 04
            //var categoryProducts = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(db, categoryProducts));

            //Problem 05
            //Console.WriteLine(GetProductsInRange(db));

            //Problem 06
            //Console.WriteLine(GetSoldProducts(db));

            //Problem 07
            //Console.WriteLine(GetCategoriesByProductsCount(db));

            //Problem 08
            Console.WriteLine(GetUsersWithProducts(db));
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportUserDto>), new XmlRootAttribute("Users"));
            var reader = new StringReader(inputXml);

            var userDtos = (List<ImportUserDto>)serializer.Deserialize(reader);

            var users = Mapper.Map<List<User>>(userDtos);
            
            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportProductDto>), new XmlRootAttribute("Products"));
            var reader = new StringReader(inputXml);

            var productDtos = (List<ImportProductDto>)serializer.Deserialize(reader);

            var products = productDtos.Select(p => new Product
            {
                Name = p.Name,
                Price = p.Price,
                SellerId = p.SellerId,
                BuyerId = p.BuyerId
            })
            .ToList();

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCategoryDto>), new XmlRootAttribute("Categories"));
            var reader = new StringReader(inputXml);

            var categoryDtos = (List<ImportCategoryDto>)serializer.Deserialize(reader);

            var categories = categoryDtos
                .Select(c => new Category
                {
                    Name = c.Name
                })
                .Where(c => c.Name != null)
                .ToList();

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCategoryProductDto>), 
                new XmlRootAttribute("CategoryProducts"));

            var reader = new StringReader(inputXml);

            var categoryproductDtos = (List<ImportCategoryProductDto>)serializer.Deserialize(reader);

            var categoryProducts = categoryproductDtos
                .Select(cp => new CategoryProduct
                {
                    CategoryId = cp.CategoryId,
                    ProductId = cp.ProductId
                })
                .Where(cp => context.Categories.Any(c => c.Id == cp.CategoryId))
                .Where(cp => context.Products.Any(p => p.Id == cp.ProductId))
                .ToList();

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var productDtos = context.Products
                .Select(p => new ExportProductInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportProductInRangeDto>), 
                new XmlRootAttribute("Products"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, productDtos, namespaces);

            var products = writer.ToString();

            return products;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var userDtos = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .Select(u => new ExportUserSoldProductDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new ExportUserProductDto 
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToList()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportUserSoldProductDto>),
                new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, userDtos, namespaces);

            var users = writer.ToString();

            return users;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categoryDtos = context.Categories
                .Select(c => new ExportCategoryByProductsCountDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts
                        .Select(cp => cp.Product)
                        .Count(),
                    AveragePrice = c.CategoryProducts
                        .Select(cp => cp.Product.Price)
                        .Average(),
                    TotalRevenue = c.CategoryProducts
                        .Select(cp => cp.Product.Price)
                        .Sum()
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportCategoryByProductsCountDto>), 
                new XmlRootAttribute("Categories"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, categoryDtos, namespaces);

            var categories = writer.ToString();

            return categories;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var userWithProductDtos = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .Select(u => new ExportUserWithProductDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportUserSoldProductRootDto()
                    {
                        Count = u.ProductsSold.Count(ps => ps.Buyer != null),
                        Products = u.ProductsSold
                        .Where(ps => ps.Buyer != null)
                        .Select(p => new ExportUserProductDto()
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .ToList()                    
                    }                    
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .Take(10)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportUserWithProductDto>),
                new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, userWithProductDtos, namespaces);

            var users = writer.ToString();

            return users;
        }

        private static void ResetDatabase(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            Console.WriteLine($"Successfully deleted!");

            context.Database.EnsureCreated();
            Console.WriteLine($"Successfully created!");
        }
    }
}