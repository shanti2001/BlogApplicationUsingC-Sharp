
using BlogApplication.Models;
using com.blogApplication.BlogApplication2.entity; 

namespace BlogApplication.Services
{
    public class PostService
    {
        public List<Post> GetAllPostsSortedByPublishDate(string sortBy)
        {
            List<Post> posts;
            using(var dbContext = new DbConfigure()){
                IQueryable<Post> postsQuery = dbContext.Posts;

                if (sortBy.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    postsQuery = postsQuery.OrderByDescending(post => post.PublishedAt);
                }
                else if (sortBy.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    postsQuery = postsQuery.OrderBy(post => post.PublishedAt);
                }
                posts = postsQuery.ToList();
            }
            return posts;
        }
    }
}
