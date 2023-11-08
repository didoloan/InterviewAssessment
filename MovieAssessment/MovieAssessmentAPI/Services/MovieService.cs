using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MovieAssessmentAPI.Models;

namespace MovieAssessmentAPI.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAsync(MoviesReq req);
        Task<Movie?> GetAsync(string id);
        Task CreateAsync(Movie newMovie);
        Task RemoveAsync(string id);
        Task<List<Movie>> SearchMovie(MovieSearch search);
        Task UpdateTicketPriceAsync(string id, decimal price);
    }
    public class MovieService : IMovieService
    {
        private readonly IMongoCollection<Movie> movieCollection;
        private readonly ProjectionDefinition<Movie> projection = Builders<Movie>.Projection.Exclude("_id");
        public MovieService(IOptions<DbConfig> movieDatabaseSettings)
        {
            var mongoClient = new MongoClient(movieDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(movieDatabaseSettings.Value.DatabaseName);
            movieCollection = mongoDatabase.GetCollection<Movie>(movieDatabaseSettings.Value.MoviesCollectionName);
        }

        public async Task<List<Movie>> GetAsync(MoviesReq req) =>
            await movieCollection.Find(_ => true).Skip(req.page <= 1 ? 0 : (req.page - 1) * req.PageSize).Limit(req.PageSize).ToListAsync();

        public async Task<Movie?> GetAsync(string id) =>
            await movieCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Movie newMovie) =>
            await movieCollection.InsertOneAsync(newMovie);

        public async Task UpdateAsync(string id, Movie updatedMovie) =>
            await movieCollection.ReplaceOneAsync(x => x.Id == id, updatedMovie);

        public async Task RemoveAsync(string id) =>
            await movieCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Movie>> SearchMovie(MovieSearch search)
        {
            var builder = Builders<Movie>.Filter;
            var filter = builder.Empty;

            if (search.minRating > 0)
            {
                filter = filter & builder.Gte(movie => movie.Rating, search.minRating);
            }
            if (search.maxRating > 0)
            {
                filter = filter & builder.Lte(movie => movie.Rating, search.maxRating);
            }

            if (search.minTicketPrice > 0)
            {
                filter = filter & builder.Gte(movie => movie.TicketPrice, search.minTicketPrice);
            }
            if (search.maxTicketPrice < 15)
            {
                filter = filter & builder.Lte(movie => movie.TicketPrice, search.maxTicketPrice);
            }

            if (!String.IsNullOrWhiteSpace(search.Name))
            {
                filter = filter & builder.Text(search.Name, new TextSearchOptions { CaseSensitive = false, DiacriticSensitive = false });
            }

            if (search.Year > 0)
            {
                filter = filter & builder.Eq(movie => movie.ReleaseDate.Year, search.Year);
            }

            var movies = await movieCollection.Find<Movie>(filter).ToListAsync();
            return movies;
        }

        public async Task UpdateTicketPriceAsync(string id, decimal price)
        {
            await movieCollection.UpdateOneAsync(new BsonDocument { { "_id", id } }, Builders<Movie>.Update.Set("ticketprice", Convert.ToDouble(Math.Round(price, 2))));
        }
    }
}