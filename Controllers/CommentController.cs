using Microsoft.AspNetCore.Mvc;
using BlogApplication.Models;
using com.blogApplication.BlogApplication2.entity;
using Microsoft.EntityFrameworkCore;
namespace BlogApplication.Controllers{
    public class CommentController :Controller{
        public IActionResult Comment(int id,string name,string email,string comment){
            using(var dbContext = new DbConfigure()){
                Post post = dbContext.Posts.Find(id);
                Comment newComment = new Comment();
                newComment.Name = name;
                newComment.Email = email;
                newComment.CommentText = comment;
                newComment.CreatedAt = DateTime.Now;
                newComment.UpdatedAt = DateTime.Now;
                newComment.Post = post;
                dbContext.Comments.Add(newComment);
                dbContext.SaveChanges();

                dbContext.SaveChanges();
            }
            return RedirectToAction("ShowPost","Post",new { id = id });
        }
        public IActionResult ShowComment(int id){
            List<Comment> postComment;
            Post post;
             using(var dbContext = new DbConfigure()){
                post = dbContext.Posts.Include(p=>p.Comments).Include(p=>p.Author).FirstOrDefault(p => p.Id == id);
                postComment = post.Comments;
             }
             ViewBag.post = post;
            return View(postComment);
        }
        public IActionResult DeleteComment(int id){
            Comment comment;
            Post post;
            using(var dbContext = new DbConfigure()){
                comment = dbContext.Comments.Include(c=>c.Post).FirstOrDefault(c=>c.Id==id);
                post = comment.Post;
                dbContext.Comments.Remove(comment);
                dbContext.SaveChanges();
            }
            return RedirectToAction("ShowComment",new {id=post.Id});
        }
        public IActionResult UpdateComment(int id){
            Comment comment;
            using(var dbContext = new DbConfigure()){
                comment = dbContext.Comments.Include(c=>c.Post).FirstOrDefault(c=>c.Id==id);
            }
            return View(comment);
        }
        public IActionResult Update(int id,string comment){
            Comment exixtComment;
            Post post;
            using(var dbContext = new DbConfigure()){
                exixtComment = dbContext.Comments.Include(c=>c.Post).FirstOrDefault(c=>c.Id==id);
                exixtComment.UpdatedAt = DateTime.Now;
                exixtComment.CommentText = comment;
                post = exixtComment.Post;
                dbContext.SaveChanges();
            }

            return RedirectToAction("ShowComment",new {id=post.Id});
        }
    }
}