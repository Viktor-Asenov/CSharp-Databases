namespace VaporStore.DataProcessor
{
	using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			var genres = context.Genres
					.Where(g => genreNames.Contains(g.Name))
					.ToList()
					.Select(g => new
					{
						Id = g.Id,
						Genre = g.Name,
						Games = g.Games.Where(gm => gm.Purchases.Any())
						.Select(gm => new
						{
							Id = gm.Id,
							Title = gm.Name,
							Developer = gm.Developer.Name,
							Tags = string.Join(", ", gm.GameTags.Select(gt => gt.Tag.Name).ToList()),
							Players = gm.Purchases.Count
						})
						.OrderByDescending(g => g.Players)
						.ThenBy(g => g.Id)
						.ToList(),
						TotalPlayers = g.Games.Sum(gm => gm.Purchases.Count)
					})
					.ToList();

			var result = JsonConvert.SerializeObject(genres, Formatting.Indented);

			return result;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
		{
            PurchaseType purchaseTypeEnum = Enum.Parse<PurchaseType>(purchaseType);

            var users = context
                .Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any()))
                .Select(u => new ExportUsersPurchasesModel()
                {
                    Username = u.Username,
                    Purchases = context
                        .Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == purchaseTypeEnum)
                        .OrderBy(p => p.Date)
                        .Select(p => new ExportPurchaseModel()
                        {
                            CardNumber = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameModel()
                            {
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .ToArray(),
                    TotalSpent = context
                        .Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == purchaseTypeEnum)
                        .Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Length > 0)
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            var result = XmlConverter.Serialize<ExportUsersPurchasesModel>(users, "Users");

            return result;
        }
	}
}