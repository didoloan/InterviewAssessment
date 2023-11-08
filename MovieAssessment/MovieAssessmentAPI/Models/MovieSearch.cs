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
        public string? Name { get; set; }
        public int Year { get; set; }
        public double minRating { get; set; } = 0;
        public double maxRating { get; set; } = 5;
        public decimal minTicketPrice { get; set; } = 0;
        public decimal maxTicketPrice { get; set; } = 15;
        public string[]? Country { get; set; }
        public string[]? Genre { get; set; }
        public BsonDocument GetSearchQuery() {
            var builder = Builders<Movie>.Filter;

            var filter = Builders<Movie>.Filter.And(new FilterDefinition<Movie>[] {
                Builders<Movie>.Filter.Gte(x => x.Rating, minRating),
                Builders<Movie>.Filter.Lte(x => x.Rating, maxRating),
                Builders<Movie>.Filter.Gte(x => x.TicketPrice, minTicketPrice),
                Builders<Movie>.Filter.Lte(x => x.TicketPrice, maxTicketPrice)
            });


            if(!String.IsNullOrWhiteSpace(Name)) {
                return Builders<Movie>.Filter.Text(Name , new TextSearchOptions {CaseSensitive = false, DiacriticSensitive = false})
                .ToBsonDocument();
            }

            return filter.ToBsonDocument();
        }
    }
}