namespace SoftJail.DataProcessor.ImportDto
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ImportPrisonersMailsModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FullName { get; set; }
        
        [Required]
        [RegularExpression(@"The\s[A-Z]{1}[a-z]+")]
        public string Nickname { get; set; }
        
        [Range(18, 65)]
        public int Age { get; set; }
        
        [Required]
        public string IncarcerationDate { get; set; }
        
        public string ReleaseDate { get; set; }
        
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }
        
        public int? CellId { get; set; }
        
        public ImportMailsModel[] Mails { get; set; }
    }
}
