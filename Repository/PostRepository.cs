using System.Data;
using BlogApplication.Models;
using com.blogApplication.BlogApplication2.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Reposotory{
    public class PostRepository : IPostRepository{
        
         public HashSet<Post> SearchPosts(string query)
        {
            string []serachWord = query.Split(' ');
            List<Post> posts = new List<Post>();
            using(var DbContext = new DbConfigure()){
                List<Post> tempPost;
                foreach(string item in serachWord){
                    tempPost = DbContext.Posts
                    .Where(p =>
                        EF.Functions.Like(p.Title, $"%{item}%") ||
                        EF.Functions.Like(p.Content, $"%{item}%") ||
                        p.Tags.Any(t => EF.Functions.Like(t.Name, $"%{query}%"))
                    )
                    .ToList();
                    posts.AddRange(tempPost);
                }
                
            }

            HashSet<Post> uniquePosts = new HashSet<Post>(posts);
            

            return uniquePosts;
        }

        
    }
}