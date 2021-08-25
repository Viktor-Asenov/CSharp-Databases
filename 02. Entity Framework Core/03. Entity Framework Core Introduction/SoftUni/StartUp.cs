using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            //Problem 03
            //Console.WriteLine(GetEmployeesFullInformation(db));

            //Problem 04
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(db));

            //Problem 05
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(db));

            //Problem 06
            //Console.WriteLine(AddNewAddressToEmployee(db));

            //Problem 07
            //Console.WriteLine(GetEmployeesInPeriod(db));

            //Problem 08
            //Console.WriteLine(GetAddressesByTown(db));

            //Problem 09
            //Console.WriteLine(GetEmployee147(db));

            //Problem 10
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(db));

            //Problem 11
            //Console.WriteLine(GetLatestProjects(db));

            //Problem 12
            //Console.WriteLine(IncreaseSalaries(db));

            //Problem 13
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(db));

            //Problem 14
            //Console.WriteLine(DeleteProjectById(db));

            //Problem 15
            Console.WriteLine(RemoveTown(db));
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var emp in context.Employees.OrderBy(e => e.EmployeeId))
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} {emp.MiddleName} {emp.JobTitle} {emp.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var employee in context.Employees.Where(e => e.Salary > 50000).OrderBy(e => e.FirstName))
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesFromResearchAndDevelopment = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Department = e.Department.Name,
                    Salary = e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (var employee in employeesFromResearchAndDevelopment)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} " +
                    $"from {employee.Department} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employeeWithLastNameNakov = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            employeeWithLastNameNakov.Address = newAddress;

            context.SaveChanges();

            var orderedEmployees = context.Employees
                .OrderByDescending(e => e.Address.AddressId)
                .Take(10)
                .Select(e => new
                {
                    AddressText = e.Address.AddressText
                })
                .ToList();

            foreach (var employee in orderedEmployees)
            {
                sb.AppendLine($"{employee.AddressText}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesInPeriod = context.Employees
                .Where(e => e.EmployeesProjects.Any(e => e.Project.StartDate.Year >= 2001
                && e.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        StartDate = ep.Project.StartDate
                            .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = ep.Project.EndDate
                            .HasValue ? ep.Project.EndDate
                            .Value
                            .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                    })
                });

            foreach (var emp in employeesInPeriod)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - Manager: {emp.ManagerFirstName} {emp.ManagerLastName}");

                foreach (var project in emp.Projects)
                {
                    sb.AppendLine($"--{project.ProjectName} - {project.StartDate} - {project.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var adresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(e => e.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .ToList();

            foreach (var address in adresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147 = context.Employees
                .Select(e => new
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name
                        })
                })
                .FirstOrDefault(e => e.EmployeeId == 147);

            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            foreach (var project in employee147.Projects
                                               .OrderBy(p => p.ProjectName))
            {
                sb.AppendLine($"{project.ProjectName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departmentsWithMoreThan5Employees = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                        .Select(e => new
                        {
                            EmployeeFirstName = e.FirstName,
                            EmployeeLastName = e.LastName,
                            JobTitle = e.JobTitle
                        })
                        .OrderBy(e => e.EmployeeFirstName)
                        .ThenBy(e => e.EmployeeLastName)
                        .ToList()
                })
                .ToList();

            foreach (var department in departmentsWithMoreThan5Employees)
            {
                sb.AppendLine($"{department.DepartmentName} – {department.ManagerFirstName} {department.ManagerLastName}");

                foreach (var employee in department.Employees)
                {
                    sb.AppendLine($"{employee.EmployeeFirstName} {employee.EmployeeLastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var lastTenStartedProjects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    ProjectName = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })

                .OrderBy(p => p.ProjectName)
                .ToList();

            foreach (var project in lastTenStartedProjects)
            {
                sb.AppendLine($"{project.ProjectName}" +
                              $"{Environment.NewLine}" +
                              $"{project.Description}" +
                              $"{Environment.NewLine}" +
                              $"{project.StartDate}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesWithIncreasedSalary = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design"
                || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Salary = e.Salary + e.Salary * 0.12M
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var employee in employeesWithIncreasedSalary)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projectToDelete = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            var employeeProjectToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2)
                .ToList();

            foreach (var employeeProject in employeeProjectToDelete)
            {
                context.EmployeesProjects.Remove(employeeProject);
            }

            context.Projects.Remove(projectToDelete);

            var projects = context.Projects
                .Take(10)
                .ToList();

            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var town = context.Towns
                .Include(t => t.Addresses)
                .FirstOrDefault(t => t.Name == "Seattle");

            var addressesIds = town.Addresses
                .Select(a => a.AddressId)
                .ToList();

            var employeesToSetAddressId = context.Employees
                .Where(e => e.AddressId.HasValue && addressesIds.Contains(e.AddressId.Value))
                .ToList();

            foreach (var employee in employeesToSetAddressId)
            {
                employee.AddressId = null;
            }

            foreach (var addressId in addressesIds)
            {
                var address = context.Addresses
                    .FirstOrDefault(a => a.AddressId == addressId);

                context.Addresses.Remove(address);
            }

            context.Towns.Remove(town);

            sb.AppendLine($"{addressesIds.Count} addresses in {town.Name} were deleted");

            return sb.ToString().TrimEnd();
        }
    }
}
