namespace BookShop.DataProcessor
{
    using System;
    using System.Linq;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors
                .ToList()
                .Select(a => new
                {
                    AuthorName = a.FirstName + ' ' + a.LastName,
                    Books = a.AuthorsBooks
                    .Select(b => new
                    {
                        BookName = b.Book.Name,
                        BookPrice = b.Book.Price.ToString("F2")
                    })
                    .OrderByDescending(b => decimal.Parse(b.BookPrice))
                    .ToList()
                })
                .OrderByDescending(a => a.Books.Count)
                .ThenBy(a => a.AuthorName)
                .ToList();

            var result = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return result;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var oldestBooks = context.Books
                .Where(b => b.PublishedOn < date && b.Genre.ToString() == "Science")                
                .Select(b => new ExportOldestBooksModel
                {
                    Name = b.Name,
                    PublishedOn = b.PublishedOn.ToString("MM/dd/yyyy"),
                    Pages = b.Pages
                })
                .OrderByDescending(b => b.Pages)
                .ThenByDescending(b => b.PublishedOn)
                .Take(10)
                .ToList();

            var result = XmlConverter.Serialize(oldestBooks, "Books");

            return result;
        }
    }
}