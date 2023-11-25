using Microsoft.AspNetCore.Mvc;
using BlogApplication.Models;
using com.blogApplication.BlogApplication2.entity;
using BlogApplication.Reposotory;
using BlogApplication.Services;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Controllers{
    
    public class PostController:Controller{
        private readonly IPostRepository _repository; 
        private readonly PostService _service;

        public PostController(IPostRepository repository,PostService postService)
        {
            _repository = repository;
            _service = postService;
        }
        public IActionResult NewPost(){
            return View();
        }
        [HttpPost]
        public IActionResult Publishpost(Post post,string tagInput){
            using (var DbContext = new DbConfigure()){
                User author = DbContext.Users.FirstOrDefault(item=> item.Id==1);
                post.Author = author;
                post.UpdatedAt = DateTime.Now;
                post.CreatedAt = DateTime.Now;
                post.PublishedAt = DateTime.Now;
                post.IsPublished = true;
                
                List<Post> posts = author.Posts;
                if(posts==null){
                    posts = new List<Post>();
                }
                posts.Add(post);
                author.Posts = posts;

                DbContext.Posts.Add(post);

                string []tagsName = tagInput.Split(',');
                List<Tag> tags = DbContext.Tags.ToList();
                List<Tag> postTags = post.Tags;

                foreach(string tagName in tagsName){
                    foreach(Tag tag in tags){
                        if(tag.Name.Equals(tagName)){
                            Tag exixtTag = DbContext.Tags.FirstOrDefault(t=> t.Id==tag.Id);
                            exixtTag.UpdatedAt = DateTime.Now;
                            List<Post> tagPosts = exixtTag.Posts;
                            postTags.Add(exixtTag);
                            tagPosts.Add(post);
                            DbContext.SaveChanges();
                        }
                    }
                    Tag myTag = DbContext.Tags.FirstOrDefault(t=>t.Name.Equals(tagName));
                    if(myTag==null){
                        Tag newTag = new Tag();
                        newTag.Name = tagName;
                        newTag.CreatedAt = DateTime.Now;
                        newTag.UpdatedAt = DateTime.Now;

                        newTag.Posts.Add(post);
                        postTags.Add(newTag);

                        DbContext.Tags.Add(newTag);
                        DbContext.SaveChanges();
                    }
                }
                
                
                DbContext.SaveChanges();


            }
            return RedirectToAction("Index","Home");
        }
        public IActionResult ShowPost(int id){
            Post post;
            using(var DbContext = new DbConfigure()){
                post = DbContext.Posts.Include(p=>p.Author).Include(p=>p.Tags).FirstOrDefault(item=> item.Id==id);

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
                post  = dbContext.Posts.Include(p=>p.Tags).FirstOrDefault(p=> p.Id==id);
                
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
        public IActionResult Update(Post post,string tagInput){
            int id = post.Id;
            Post exixtPost;
            using(var dbContext = new DbConfigure()){
                exixtPost  = dbContext.Posts.Include(p=>p.Tags).FirstOrDefault(p=>p.Id==post.Id);
                if(exixtPost!=null){
                    exixtPost.Title = post.Title;
                    exixtPost.Excerpt = post.Excerpt;
                    exixtPost.Content = post.Content;

                    string []tagsName = tagInput.Split(',');
                    List<Tag> tags = dbContext.Tags.ToList();
                    List<Tag> postTags = exixtPost.Tags;

                    foreach(string tagName in tagsName){
                        foreach(Tag tag in tags){
                            if(tag.Name.Equals(tagName)){
                                Tag exixtTag = dbContext.Tags.FirstOrDefault(t=> t.Id==tag.Id);
                                exixtTag.UpdatedAt = DateTime.Now;
                                List<Post> tagPosts = exixtTag.Posts;

                                if(!postTags.Contains(exixtTag)){
                                    postTags.Add(exixtTag);
                                    tagPosts.Add(post);
                                }
                                
                                dbContext.SaveChanges();
                            }
                        }
                        Tag myTag = dbContext.Tags.FirstOrDefault(t=>t.Name.Equals(tagName));
                        if(myTag==null){
                            Tag newTag = new Tag();
                            newTag.Name = tagName;
                            newTag.CreatedAt = DateTime.Now;
                            newTag.UpdatedAt = DateTime.Now;

                            newTag.Posts.Add(exixtPost);
                            postTags.Add(newTag);

                            dbContext.Tags.Add(newTag);
                            dbContext.SaveChanges();
                        }
                    }

                    dbContext.SaveChanges();

                }
            }
            return RedirectToAction("UserPost","Home");
            
        }
        public IActionResult SearchPost(string q){
            
            HashSet<Post> posts = _repository.SearchPosts(q);
            return View(posts);
        }
        public IActionResult Sort(string sort){
            List<Post> posts = _service.GetAllPostsSortedByPublishDate(sort);
            return View(posts);
        }
        [HttpGet("/Post/showFilter")]
        public IActionResult ShowFilter(){
            List<User> users;
            List<Tag> tags;
            using(var DbContext = new DbConfigure()){
                users = DbContext.Users.ToList();
                tags = DbContext.Tags.ToList();
            }
            ViewBag.users = users;
            ViewBag.tags = tags;
            return View();
        }
        public IActionResult FilterBy(string []authorsName,string []tagsId){

            HashSet<Post> posts = new HashSet<Post>();
            using(var dbContext = new DbConfigure()){
                List<User> users = dbContext.Users.Include(u=>u.Posts).ToList();
                List<Tag> tags = dbContext.Tags.Include(t=>t.Posts).ToList();
                if(tagsId.Length==0 && authorsName.Length==0){
                    List<Post> post = dbContext.Posts.Include(p=>p.Author).Include(p=>p.Tags).ToList();
                    posts = new HashSet<Post>(post);
                }
                else if (tagsId.Length==0)
                {
                    foreach (var authorName in authorsName)
                    {
                        foreach (var user in users)
                        {
                            if (user.Name.Equals(authorName))
                            {
                                posts.UnionWith(user.Posts);
                            }
                        }
                    }
                }
                else if (authorsName.Length==0)
                {
                    foreach (var tagId in tagsId)
                    {
                        var tag = dbContext.Tags.Include(t=>t.Posts).FirstOrDefault(t=>t.Id==int.Parse(tagId));
                        if (tags.Contains(tag))
                        {
                            posts.UnionWith(tag.Posts);
                        }
                    }
                }
                else
                {
                    foreach (var authorName in authorsName)
                    {
                        foreach (var user in users)
                        {
                            if (user.Name.Equals(authorName))
                            {
                                var authorPosts = user.Posts;
                                foreach (var post in authorPosts)
                                {
                                    var postTags = post.Tags;
                                    foreach (var tagId in tagsId)
                                    {
                                        var tag = dbContext.Tags.Include(t=>t.Posts).FirstOrDefault(t=>t.Id==int.Parse(tagId));
                                        if (tags.Contains(tag) && postTags.Contains(tag))
                                        {
                                            posts.Add(post);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return View(posts);
        }

    }
    
    
}