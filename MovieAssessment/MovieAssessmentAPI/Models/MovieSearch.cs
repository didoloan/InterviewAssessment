using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MovieAssessmentAPI.Models
{
    public class MovieSearch
    {
        [StringLength(maximumLength:60)]
        public string? Name { get; set; }
        public int Year { get; set; }
        public double minRating { get; set; } = 0;
        public double maxRating { get; set; } = 5;
        public decimal minTicketPrice { get; set; } = 0;
        public decimal maxTicketPrice { get; set; } = 15;
        public string[]? Country { get; set; }
        public string[]? Genre { get; set; }
        public FilterDefinition<Movie> GetSearchQuery() {
            var builder = Builders<Movie>.Filter;
            var filter = builder.Empty;

            if (minRating > 0)
            {
                filter = filter & builder.Gte(movie => movie.Rating, minRating);
            }
            if (maxRating > 0)
            {
                filter = filter & builder.Lte(movie => movie.Rating, maxRating);
            }

            if (minTicketPrice > 0)
            {
                filter = filter & builder.Gte(movie => movie.TicketPrice, minTicketPrice);
            }
            if (maxTicketPrice < 15)
            {
                filter = filter & builder.Lte(movie => movie.TicketPrice, maxTicketPrice);
            }

            if (!String.IsNullOrWhiteSpace(Name))
            {
                filter = filter & builder.Text(Name, new TextSearchOptions { CaseSensitive = false, DiacriticSensitive = false });
            }

            if (Year > 0)
            {
                filter = filter & builder.Gte(movie => movie.ReleaseDate, new DateTime(Year, 1, 1, 0, 0, 0));
                filter = filter & builder.Lt(movie => movie.ReleaseDate, new DateTime(Year+1, 1, 1, 0, 0, 0));
            }

            return filter;
        }
    }
}