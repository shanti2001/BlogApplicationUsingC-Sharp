using System.Collections.Generic;
using BlogApplication.Models;

namespace BlogApplication.Reposotory
{
    public interface IPostRepository
    {
        HashSet<Post> SearchPosts(string query);
        
    }
}
