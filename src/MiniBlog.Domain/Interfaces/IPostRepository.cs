using MiniBlog.Domain.Entities;

namespace MiniBlog.Domain.Interfaces
{
    public interface IPostRepository
    {
        IQueryable<Post> GetAllQueryable();
        Task<Post?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Post post, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
