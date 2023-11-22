using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApplication.Models
{
    [Table("posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Excerpt { get; set; }
        
        [Column(TypeName = "nvarchar(4000)")]
        public string Content { get; set; }

        
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public User Author { get; set; }

        public DateTime PublishedAt { get; set; }

        public bool IsPublished { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Comment> Comments { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        
        public Post() 
        {
            
            Tags = new List<Tag>();
            Comments = new List<Comment>();
        }
    }
}
