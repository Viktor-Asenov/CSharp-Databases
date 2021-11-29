namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Collections.Generic;

    public class ExportEmployeesDto
    {
        public string Username { get; set; }

        public List<ExportTaskDto> Tasks { get; set; }
    }
}
