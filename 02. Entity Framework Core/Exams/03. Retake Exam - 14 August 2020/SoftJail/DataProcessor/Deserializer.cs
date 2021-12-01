namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentCellsDtos = JsonConvert.DeserializeObject<ImportDepartmentCellsModel[]>(jsonString);

            var sb = new StringBuilder();
            var departments = new List<Department>();

            foreach (var dto in departmentCellsDtos)
            {
                if (!IsValid(dto) || !dto.Cells.All(IsValid)
                    || !dto.Cells.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var department = new Department
                {
                    Name = dto.Name
                };

                foreach (var cellDto in dto.Cells)
                {
                    var cell = new Cell
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    };

                    department.Cells.Add(cell);
                }

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersMailsDtos = JsonConvert.DeserializeObject<ImportPrisonersMailsModel[]>(jsonString);

            var sb = new StringBuilder();
            var prisoners = new List<Prisoner>();

            foreach (var dto in prisonersMailsDtos)
            {
                if (!IsValid(dto) || !dto.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var incarcerationDate = DateTime.ParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var isValidReleaseDate = DateTime.TryParseExact(dto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out DateTime releaseDate);

                var prisoner = new Prisoner
                {
                    FullName = dto.FullName,
                    Nickname = dto.Nickname,
                    Age = dto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = isValidReleaseDate ? (DateTime?)releaseDate : null,
                    Bail = dto.Bail,
                    CellId = dto.CellId
                };

                foreach (var mailDto in dto.Mails)
                {
                    var mail = new Mail
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address
                    };

                    prisoner.Mails.Add(mail);
                }

                prisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var officerprisonersModel = XmlConverter.Deserializer<ImportOfficersPrisonersModel>(xmlString, "Officers");

            var sb = new StringBuilder();
            var officers = new List<Officer>();

            foreach (var dto in officerprisonersModel)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var officer = new Officer
                {
                    FullName = dto.Name,
                    Salary = dto.Money,
                    Position = Enum.Parse<Position>(dto.Position),
                    Weapon = Enum.Parse<Weapon>(dto.Weapon),
                    DepartmentId = dto.DepartmentId,
                    OfficerPrisoners = dto.Prisoners.Select(p => new OfficerPrisoner
                    {
                        PrisonerId = p.Id
                    }).ToList()
                };

                officers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}