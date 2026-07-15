using Microsoft.EntityFrameworkCore;
using MiniBlog.Domain.Entities;
using MiniBlog.Domain.Interfaces;
using MiniBlog.Infrastructure.Persistence;

namespace MiniBlog.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Post> GetAllQueryable()
        {
            return _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .AsNoTracking();
        }

        public async Task<Post?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
        {
            await _context.Posts.AddAsync(post, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
