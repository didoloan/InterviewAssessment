using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieAssessmentAPI.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("releasedate")]
        public DateTime ReleaseDate { get; set; }
        [BsonElement("rating")]
        public double Rating { get; set; }
        [BsonElement("ticketprice")]
        public decimal TicketPrice { get; set; }
        [BsonElement("country")]
        public string[] Country { get; set; }
        [BsonElement("genre")]
        public string[] Genre { get; set; }
        [BsonElement("photo")]
        public string Photo { get; set; }
    }
}