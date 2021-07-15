using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PederivaArticles.Models;
using PederivaArticles.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
namespace PederivaArticles.Controllers
{
    [Authorize]
    public class ArticleController : Controller 
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ArticleService _artSvc;
        private readonly LikeService _likeSvc;

        public ArticleController(ArticleService articleService, LikeService likeService,  IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
            _artSvc = articleService;
            _likeSvc = likeService;
        }

        [AllowAnonymous]
        public ActionResult<IList<Article>> Index(){
            IList<Article> newArticles = new List<Article>();
            var ret =_artSvc.Read();
            foreach(var i in ret){
                i.count = _likeSvc.Find(i.Id).Count();
                newArticles.Add(i);
            }
            return View(newArticles);
        }

        [HttpGet]
        public ActionResult Create() => View();
        

        private string UploadedFile(ArticleViewModel model)  
        {  
            string uniqueFileName = null;  
  
            if (model.Picture != null)  
            {  
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");  
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;  
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);  
                using (var fileStream = new FileStream(filePath, FileMode.Create))  
                {  
                    model.Picture.CopyTo(fileStream);  
                }  
            }  
            return uniqueFileName;  
        }  

        [HttpPost]
        public JsonResult LikesCountFirst(string id)
        {
            var Coleccion = _likeSvc.Find(id);
            return Json(Coleccion.Count());
        }


        [HttpPost]
        public JsonResult LikesCount(string id)
        {
            string UserId = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier).Value;
            Like Exist = _likeSvc.Find(id, UserId);
            if(Exist != null)
            {
                _likeSvc.Delete(Exist.Id);
            }
            else
            {
                Exist = new Like();
                Exist.UserId = UserId;
                Exist.ArticleId = id;
                _likeSvc.Create(Exist);
            }
            var Coleccion = _likeSvc.Find(id);
            LikeUser resultado = new LikeUser();
            resultado.ArticleId = id;
            resultado.UserId = false;
            foreach(var i in Coleccion){
                if(i.UserId == UserId)
                {
                    resultado.UserId = true;
                }            
            }
            resultado.Count = Coleccion.Count();
            return Json(resultado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create(ArticleViewModel model){

            string uniqueFileName = UploadedFile(model);  
            Article article = new Article  
                            {  
                                UserId = model.UserId,  
                                UserName = model.UserName,  
                                Title = model.Title,  
                                Short = model.Short,  
                                Content = model.Content,  
                                Created = model.Created,  
                                LastUpdated = model.LastUpdated,  
                                IP = model.IP,  
                                Picture = uniqueFileName,  
                            };  
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            article.IP =  remoteIpAddress.ToString();
            article.Created = article.LastUpdated = DateTime.Now;
            article.UserId = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier).Value;
            article.UserName = User.Identity.Name;
            if(ModelState.IsValid){
                _artSvc.Create(article);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult<Article> View(string id) => 
            View(_artSvc.Find(id));

        [HttpGet]
        public ActionResult<Article> Edit(string id) => 
            View(_artSvc.Find(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Article article)
        {
            Article first =_artSvc.Find(article.Id);
            article.Picture = first.Picture;
            article.LastUpdated = DateTime.Now;
            article.Created = article.Created.ToLocalTime();
            if(ModelState.IsValid){
                if(User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier).Value != article.UserId)
                {
                    return Unauthorized();
                }
                _artSvc.Update(article);
                return RedirectToAction("Index");
            }
            return View(article);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            _artSvc.Delete(id);
            return RedirectToAction("Index");
        }
    }
}