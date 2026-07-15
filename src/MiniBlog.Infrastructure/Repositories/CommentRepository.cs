using Microsoft.EntityFrameworkCore;
using MiniBlog.Domain.Entities;
using MiniBlog.Domain.Interfaces;
using MiniBlog.Infrastructure.Persistence;

namespace MiniBlog.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context) => _context = context;

        public async Task<List<Comment>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .Include(c => c.Author)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            await _context.Comments.AddAsync(comment, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }

}