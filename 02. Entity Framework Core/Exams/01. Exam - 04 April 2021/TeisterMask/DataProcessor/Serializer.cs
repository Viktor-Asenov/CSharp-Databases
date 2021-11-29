namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var exportProjectsDtos = context.Projects
                .Where(p => p.Tasks.Count() >= 1)
                .ToList()
                .Select(p => new ExportProjectDto
                {
                    Name = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    TasksCount = p.Tasks.Count,
                    Tasks = p.Tasks.Select(t => new ExportProjectTaskDto
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(ept => ept.Name)
                    .ToList()
                })
                .OrderByDescending(ep => ep.TasksCount)
                .ThenBy(ep => ep.Name)
                .ToList();

            var result = XmlConverter.Serialize<List<ExportProjectDto>>(exportProjectsDtos, "Projects");

            return result;
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var exportEmployeesDtos = context.Employees                
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .ToList()
                .Select(e => new ExportEmployeesDto
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks.Where(et => et.Task.OpenDate >= date)
                    .OrderByDescending(et => et.Task.DueDate)
                    .ThenBy(et => et.Task.Name)
                    .Select(et => new ExportTaskDto 
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    })                    
                    .ToList()
                })
                .OrderByDescending(e => e.Tasks.Count)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToList();

            var result = JsonConvert.SerializeObject(exportEmployeesDtos, Formatting.Indented);

            return result;
        }
    }
}