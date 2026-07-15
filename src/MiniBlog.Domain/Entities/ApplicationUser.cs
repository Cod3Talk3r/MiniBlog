namespace MiniBlog.Domain.Entities
{
    public class ApplicationUser
    {
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
