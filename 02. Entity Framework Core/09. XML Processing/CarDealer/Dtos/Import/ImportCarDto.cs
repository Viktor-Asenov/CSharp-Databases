﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Car")]
    public class ImportCarDto
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public List<ImportPartCarDto> Parts { get; set; }
    }
}
