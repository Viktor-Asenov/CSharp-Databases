﻿using P01_StudentSystem.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(2048)")]
        public string Url { get; set; }

        public ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
