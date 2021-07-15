using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace PederivaArticles.Models
{

    public class ArticleViewModel  
    {  
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        
        public string Title { get; set; }
        public string Short { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public string IP { get; set; }
        [Required(ErrorMessage = "Please choose profile image")]  
        [Display(Name = "Picture")]  
        public IFormFile Picture { get; set; }  
    }  


    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        
        public string Title { get; set; }
        public string Short { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public string IP { get; set; }
        
        public string Picture { get; set; } 
        
        public int count { get; set; }
    }
}
