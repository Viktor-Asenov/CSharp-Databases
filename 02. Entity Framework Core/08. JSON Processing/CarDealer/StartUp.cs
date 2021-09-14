using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        private const string DATASETS_DIRECTORY_PATH = @"...\...\CarDealer\Datasets";

        private const string RESULT_DIRECTORY_PATH = @"...\...\CarDealer\Datasets\Result";

        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //Problem 01
            //var suppliers = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/suppliers.json");
            //Console.WriteLine(ImportSuppliers(db, suppliers));

            //Problem 02
            //var parts = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/parts.json");
            //Console.WriteLine(ImportParts(db, parts));

            //Problem 03
            //var cars = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/cars.json");
            //Console.WriteLine(ImportCars(db, cars));

            //Problem 04
            //var customers = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/customers.json");
            //Console.WriteLine(ImportCustomers(db, customers));

            //Problem 05
            //var sales = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/sales.json");
            //Console.WriteLine(ImportSales(db, sales));

            //Problem 06
            //var customers = GetOrderedCustomers(db);
            //File.WriteAllText($"{RESULT_DIRECTORY_PATH}/ordered-customers.json", customers);

            //Problem 07
            //var cars = GetCarsFromMakeToyota(db);
            //File.WriteAllText($"{RESULT_DIRECTORY_PATH}/toyota-cars.json", cars);

            //Problem 08
            //var suppliers = GetLocalSuppliers(db);
            //File.WriteAllText($"{RESULT_DIRECTORY_PATH}/local-suppliers.json", suppliers);

            //Problem 08
            //var cars = GetCarsWithTheirListOfParts(db);
            //File.WriteAllText($"{RESULT_DIRECTORY_PATH}/cars-parts.json", cars);

            //Problem 09
            //var customers = GetTotalSalesByCustomer(db);
            //File.WriteAllText($"{RESULT_DIRECTORY_PATH}/customer-sales.json", customers);

            //Problem 10
            var sales = GetSalesWithAppliedDiscount(db);
            File.WriteAllText($"{RESULT_DIRECTORY_PATH}/sales-with-applied-discount.json", sales);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var supplierIds = context.Suppliers
                .Select(s => s.Id)
                .ToList();

            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson)
                .Where(s => supplierIds.Contains(s.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDto = JsonConvert.DeserializeObject<IEnumerable<CarDto>>(inputJson);

            var listOfCars = new List<Car>();
            foreach (var car in carsDto)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };

                foreach (var partId in car.PartsId.Distinct())
                {
                    currentCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                listOfCars.Add(currentCar);
            }

            context.Cars.AddRange(listOfCars);

            context.SaveChanges();

            return $"Successfully imported {listOfCars.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var orderedCustomers = context.Customers                
                .OrderBy(c => c.BirthDate)
                .ThenByDescending(c => c.IsYoungDriver == false)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = $"{c.BirthDate.ToString("dd/MM/yyyy")}",
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();

            var json = JsonConvert.SerializeObject(orderedCustomers, Formatting.Indented);

            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .ProjectTo<SupplierDto>()
                .ToList();

            var json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    parts = c.PartCars.Select(pc => new
                    {
                        Name = pc.Part.Name,
                        Price = $"{pc.Part.Price:f2}"
                    })
                    .ToList()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .ProjectTo<CustomerTotalSaleDto>()
                .Where(c => c.BoughtCars > 0)
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return json;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new CustomerSaleDto()
                {
                    Car = new CarSaleDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    FullName = s.Customer.Name,
                    Discount = s.Discount.ToString("f2"),
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString("f2"),
                    PriceWithDiscount = (s.Car.PartCars.Sum(pc => pc.Part.Price) -
                                            s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100).ToString("f2")
                })
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return json;
        }
    }
}