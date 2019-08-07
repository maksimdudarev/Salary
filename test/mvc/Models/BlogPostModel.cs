using System.Collections.Generic;

namespace MD.Salary.WebMvc.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Number { get; set; }

        public ICollection<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}