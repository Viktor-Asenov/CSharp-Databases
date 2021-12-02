﻿namespace BookShop.DataProcessor.ImportDto
{
    using BookShop.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Book")]
    public class ImportBooksModel
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }
        
        [XmlElement("Genre")]
        [Required]
        [EnumDataType(typeof(Genre))]
        public string Genre { get; set; }
        
        [XmlElement("Price")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        
        [XmlElement("Pages")]
        [Range(50, 5000)]
        public int Pages { get; set; }
        
        [XmlElement("PublishedOn")]
        [Required]
        public string PublishedOn { get; set; }
    }
}
