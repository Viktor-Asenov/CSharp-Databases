using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class ImportCastsModel
    {
        [XmlElement("FullName")]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string FullName { get; set; }

        [XmlElement("IsMainCharacter")]
        [Required]        
        public string IsMainCharacter { get; set; }
        
        [XmlElement("PhoneNumber")]
        [Required]
        [RegularExpression(@"[+][\d]{2}-[\d]{2}-[\d]{3}-[\d]{4}")]
        public string PhoneNumber { get; set; }
        
        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }
}
