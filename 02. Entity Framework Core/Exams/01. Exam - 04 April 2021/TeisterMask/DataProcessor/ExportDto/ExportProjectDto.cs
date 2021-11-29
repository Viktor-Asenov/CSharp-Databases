namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ExportProjectDto
    {
        [XmlElement("ProjectName")]
        public string Name { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlArray("Tasks")]
        public List<ExportProjectTaskDto> Tasks { get; set; }
    }
}
