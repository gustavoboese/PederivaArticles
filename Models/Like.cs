using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace PederivaArticles.Models
{
    public class LikeUser{
        public string ArticleId { get; set; }
        public bool UserId { get; set; }
        public int Count { get; set; }
    }

    public class Like
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ArticleId { get; set; }
        public string UserId { get; set; }
    }
}
