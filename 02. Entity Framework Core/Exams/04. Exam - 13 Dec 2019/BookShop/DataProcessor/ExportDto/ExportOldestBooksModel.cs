namespace BookShop.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Book")]
    public class ExportOldestBooksModel
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        
        [XmlElement("Date")]
        public string PublishedOn { get; set; }
        
        [XmlAttribute("Pages")]
        public int Pages { get; set; }
    }
}
