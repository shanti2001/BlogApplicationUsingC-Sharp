using Microsoft.AspNetCore.Mvc;
using BlogApplication.Models;
using com.blogApplication.BlogApplication2.entity;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlogApplication.Controllers{
    
    public class PostController:Controller{
        public IActionResult NewPost(){
            return View();
        }
        [HttpPost]
        public IActionResult Publishpost(Post post){
            using (var DbContext = new DbConfigure()){
                User author = DbContext.Users.FirstOrDefault(item=> item.Id==1);
                post.Author = author;
                post.UpdatedAt = DateTime.Now;
                post.CreatedAt = DateTime.Now;
                post.PublishedAt = DateTime.Now;
                post.IsPublished = true;

                DbContext.Posts.Add(post);
                DbContext.SaveChanges();

            }
            return RedirectToAction("Index","Home");
        }
        public IActionResult ShowPost(int id){
            Post post;
            using(var DbContext = new DbConfigure()){
                post = DbContext.Posts.FirstOrDefault(item=> item.Id==id);

            }
            return View(post);
        }
        public IActionResult DeletePost(int id){
            using(var dbContext = new DbConfigure()){
                Post deletePost = dbContext.Posts.Find(id);
                if(deletePost==null){
                    Console.WriteLine("Not exixt");
                }
                else{
                    dbContext.Posts.Remove(deletePost);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("UserPost","Home");

        }
        public IActionResult UpdatePost(int id){
            Post post;
            using(var dbContext = new DbConfigure()){
                post  = dbContext.Posts.Find(id);
                
            }
            List<Tag> tags = post.Tags;
            String allTags = "";
            for(int index=0;index<tags.Count;index++) {
                if(index==tags.Count-1) {
                    allTags+=tags[index].Name;
                }
                else {
                    allTags+=tags[index].Name+",";
                }
            }
            ViewBag.AllTags = allTags;

            return View(post);
        }
        [HttpPost]
        public IActionResult Update(Post post,string allTags){
            int id = post.Id;
            Post exixtPost;
            using(var dbContext = new DbConfigure()){
                exixtPost  = dbContext.Posts.Find(id);
                if(exixtPost!=null){
                    exixtPost.Title = post.Title;
                    exixtPost.Excerpt = post.Excerpt;
                    exixtPost.Content = post.Content;
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("UserPost","Home");
            
        }
    }
    
    
}