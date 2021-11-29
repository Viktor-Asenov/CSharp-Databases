namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class ImportUsersModel
    {
		[Required]
		[RegularExpression(@"[A-Z][a-z]+\s[A-Z][a-z]+")]
        public string FullName { get; set; }

        [Required]
		[StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

		[Required]
        public string Email { get; set; }

		[Range(3, 103)]
        public int Age { get; set; }

        public ImportCardModel[] Cards { get; set; }
	}
}
