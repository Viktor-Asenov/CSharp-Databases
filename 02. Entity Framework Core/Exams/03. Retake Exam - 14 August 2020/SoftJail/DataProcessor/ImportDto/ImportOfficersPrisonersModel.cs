namespace SoftJail.DataProcessor.ImportDto
{
    using SoftJail.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Officer")]
    public class ImportOfficersPrisonersModel
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Money")]
        public decimal Money { get; set; }

        [XmlElement("Position")]
        [EnumDataType(typeof(Position))]
        public string Position { get; set; }

        [XmlElement("Weapon")]
        [EnumDataType(typeof(Weapon))]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId)")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public ImportPrisonerModel[] Prisoners { get; set; }
    }
}
