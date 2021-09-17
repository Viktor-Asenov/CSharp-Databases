using CarDealer.Data;
using CarDealer.Dto.Import;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private const string DATASETS_DIRECTORY_PATH = @"./Datasets";

        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            //ResetDatabase(db);

            //Problem 01
            //var suppliers = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(db, suppliers));

            //Problem 02
            //var parts = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/parts.xml");
            //Console.WriteLine(ImportParts(db, parts));

            //Problem 03
            //var cars = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/cars.xml");
            //Console.WriteLine(ImportCars(db, cars));

            //Problem 04
            //var customers = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/customers.xml");
            //Console.WriteLine(ImportCustomers(db, customers));

            //Problem 05
            //var sales = File.ReadAllText($"{DATASETS_DIRECTORY_PATH}/sales.xml");
            //Console.WriteLine(ImportSales(db, sales));

            //Problem 06
            //Console.WriteLine(GetCarsWithDistance(db));

            //Problem 07
            //Console.WriteLine(GetCarsFromMakeBmw(db));

            //Problem 08
            //Console.WriteLine(GetLocalSuppliers(db));

            //Problem 09
            //Console.WriteLine(GetCarsWithTheirListOfParts(db));

            //Problem 10
            //Console.WriteLine(GetTotalSalesByCustomer(db));

            //Problem 11
            Console.WriteLine(GetSalesWithAppliedDiscount(db));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportSupplierDto>), new XmlRootAttribute("Suppliers"));
            var reader = new StringReader(inputXml);

            var supplierDtos = (List<ImportSupplierDto>)serializer.Deserialize(reader);

            var suppliers = supplierDtos
                .Select(s => new Supplier()
                {
                    Name = s.Name,
                    IsImporter = s.IsImporter
                })
                .ToList();

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportPartDto>), new XmlRootAttribute("Parts"));
            var reader = new StringReader(inputXml);

            var partDtos = (List<ImportPartDto>)serializer.Deserialize(reader);

            var parts = partDtos
                .Where(p => Enumerable
                    .Range(context.Suppliers.Min(s => s.Id), context.Suppliers.Max(s => s.Id))
                    .Contains(p.SupplierId))
                .Select(p => new Part()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierId = p.SupplierId
                })
                .ToList();

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCarDto>), new XmlRootAttribute("Cars"));
            var reader = new StringReader(inputXml);

            var cars = new List<Car>();
            var carDtos = (List<ImportCarDto>)serializer.Deserialize(reader);

            var allParts = context.Parts.Select(p => p.Id).ToList();

            foreach (var carDto in carDtos)
            {
                var distinctedParts = carDto.Parts.Select(p => p.Id).Distinct();
                var parts = distinctedParts.Intersect(allParts);

                var car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance
                };

                foreach (var part in parts)
                {
                    var partCar = new PartCar()
                    {
                        PartId = part
                    };

                    car.PartCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCustomerDto>), new XmlRootAttribute("Customers"));
            var reader = new StringReader(inputXml);

            var customerDto = (List<ImportCustomerDto>)serializer.Deserialize(reader);

            var customers = customerDto
                .Select(cdto => new Customer()
                {
                    Name = cdto.Name,
                    BirthDate = cdto.BirthDate,
                    IsYoungDriver = cdto.IsYoungDriver
                })
                .ToList();

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportSaleDto>), new XmlRootAttribute("Sales"));
            var reader = new StringReader(inputXml);

            var saleDtos = (List<ImportSaleDto>)serializer.Deserialize(reader);

            var sales = saleDtos
                .Where(sdto => context.Cars.Any(c => c.Id == sdto.CarId))
                .Select(sdto => new Sale()
                {
                    CarId = sdto.CarId,
                    CustomerId = sdto.CustomerId,
                    Discount = sdto.Discount
                })
                .ToList();

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carDtos = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Select(c => new ExportCarDto() 
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance
                })
                .OrderBy(cdto => cdto.Make)
                .ThenBy(cdto => cdto.Model)
                .Take(10)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportCarDto>),
                new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, carDtos, namespaces);

            var result = writer.ToString();

            return result;
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmwCarDtos = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new ExportBmwCarDto()
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(cdto => cdto.Model)
                .ThenByDescending(cdto => cdto.TravelledDistance)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportBmwCarDto>),
                new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, bmwCarDtos, namespaces);

            var result = writer.ToString();

            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSupplierDtos = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new ExportLocalSuppliers()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportLocalSuppliers>),
                new XmlRootAttribute("suppliers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, localSupplierDtos, namespaces);

            var result = writer.ToString();

            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carWithPartsDtos = context.Cars
                .Select(c => new ExportCarWithPartsDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(pc => new ExportPartDto()
                    {
                        Name = pc.Part.Name,
                        Price =  pc.Part.Price
                    })
                    .OrderByDescending(pdto => pdto.Price)
                    .ToList()
                })
                .OrderByDescending(cdto => cdto.TraveledDistance)
                .ThenBy(cdto => cdto.Model)
                .Take(5)                
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportCarWithPartsDto>),
                new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, carWithPartsDtos, namespaces);

            var result = writer.ToString();

            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new ExportTotalSalesCustomerDto()
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales
                        .Select(s => s.Car.PartCars
                            .Select(pc => pc.Part)
                            .Sum(pc => pc.Price))
                        .Sum()
                })
                .OrderByDescending(cdto => cdto.SpentMoney)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportTotalSalesCustomerDto>),
                new XmlRootAttribute("customers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, customers, namespaces);

            var result = writer.ToString();

            return result;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new ExportSaleWithAppliedDiscountDto() 
                {
                    Car = new ExportCarWithAttributesDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price)
                        - s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100
                })
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportSaleWithAppliedDiscountDto>),
                new XmlRootAttribute("sales"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, sales, namespaces);

            var result = writer.ToString();

            return result;
        }

        private static void ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            Console.WriteLine($"Successfully deleted!");

            context.Database.EnsureCreated();
            Console.WriteLine($"Successfully created!");
        }
    }
}