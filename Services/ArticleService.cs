using System.Collections.Generic;
using MongoDB.Driver;
using PederivaArticles.Models;
using System.Linq;

namespace PederivaArticles.Services
{

    public class ArticleService
    {
        private readonly IMongoCollection<Article> _articles;

        public ArticleService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _articles = database.GetCollection<Article>("Articles");
        }

        public Article Create(Article article)
        {
            _articles.InsertOne(article);
            return article;
        }

        public IList<Article> Read() =>
            _articles.Find(article => true).ToList();

        public Article Find(string id) =>
            _articles.Find(article=>article.Id == id).SingleOrDefault();

        public void Update(Article article) =>
            _articles.ReplaceOne(sub => sub.Id == article.Id, article);

        public void Delete(string id) =>
            _articles.DeleteOne(article => article.Id == id);
    }
}