using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class ExportUserSoldProductRootDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public List<ExportUserProductDto> Products { get; set; }
    }
}
