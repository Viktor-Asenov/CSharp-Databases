namespace SoftJail.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Prisoner")]
    public class ImportPrisonerModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}