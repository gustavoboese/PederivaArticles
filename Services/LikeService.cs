using System.Collections.Generic;
using MongoDB.Driver;
using PederivaArticles.Models;
using System.Linq;

namespace PederivaArticles.Services
{

    public class LikeService
    {
        private readonly IMongoCollection<Like> _likes;

        public LikeService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _likes = database.GetCollection<Like>("Likes");
        }

        public Like Create(Like like)
        {
            _likes.InsertOne(like);
            return like;
        }

        public IList<Like> Find(string id) =>
            _likes.Find(like => like.ArticleId == id).ToList();

        public Like Find(string ArticleId, string UserId) =>
            _likes.Find(like => like.ArticleId == ArticleId && like.UserId == UserId).SingleOrDefault();

        public IList<Like> Read() =>
            _likes.Find(like => true).ToList();



        public void Update(Like like) =>
            _likes.ReplaceOne(sub => sub.Id == like.Id, like);

        public void Delete(string id) =>
            _likes.DeleteOne(like => like.Id == id);
    }
}