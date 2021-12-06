﻿using System.ComponentModel.DataAnnotations;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheatresTicketsModel
    {
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; }
        
        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Director { get; set; }
        
        public ImportTicketsModel[] Tickets { get; set; }
    }
}
