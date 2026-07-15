namespace MiniBlog.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string AuthorId { get; set; } = default!;
        public ApplicationUser Author { get; set; } = default!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
