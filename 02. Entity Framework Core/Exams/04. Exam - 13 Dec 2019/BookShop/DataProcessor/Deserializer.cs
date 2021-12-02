namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var booksDtos = XmlConverter.Deserializer<ImportBooksModel>(xmlString, "Books");

            var sb = new StringBuilder();
            var books = new List<Book>();

            foreach (var bookDto in booksDtos)
            {
                if(!IsValid(bookDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var publishedOn = DateTime.ParseExact(bookDto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                var book = new Book
                {
                    Name = bookDto.Name,
                    Genre = Enum.Parse<Genre>(bookDto.Genre),
                    Price = bookDto.Price,
                    Pages = bookDto.Pages,
                    PublishedOn = publishedOn,
                };

                books.Add(book);
                sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsDtos = JsonConvert.DeserializeObject<ImportAuthorsModel[]>(jsonString);

            var sb = new StringBuilder();
            var authors = new List<Author>();
            var emails = new List<string>();

            foreach (var authorDto in authorsDtos)
            {
                if (!IsValid(authorDto) || !authorDto.Books.All(IsValid) 
                    || !authorDto.Books.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (emails.Contains(authorDto.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var author = new Author
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Phone = authorDto.Phone,
                    Email = authorDto.Email
                };

                emails.Add(author.Email);

                foreach (var bookDto in authorDto.Books)
                {
                    var book = context.Books.FirstOrDefault(b => b.Id == bookDto.Id);
                    if (book == null)
                    {
                        continue;
                    }

                    author.AuthorsBooks.Add(new AuthorBook { Book = book });
                }

                if (author.AuthorsBooks.Count() == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    emails.Remove(author.Email);
                    continue;
                }

                var authorName = author.FirstName + " " + author.LastName;

                authors.Add(author);
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, authorName, author.AuthorsBooks.Count));
            }

            context.Authors.AddRange(authors);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}