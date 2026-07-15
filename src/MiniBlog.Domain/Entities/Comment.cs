using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBlog.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int PostId { get; set; }
        public Post Post { get; set; } = default!;

        public string AuthorId { get; set; } = default!;
        public ApplicationUser Author { get; set; } = default!;
    }
}
