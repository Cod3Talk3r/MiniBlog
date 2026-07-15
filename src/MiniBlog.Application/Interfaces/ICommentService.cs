using MiniBlog.Application.DTOs;

namespace MiniBlog.Application.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default);

        Task<CommentDto> AddAsync(int postId, CreateCommentDto dto, string authorId, CancellationToken cancellationToken = default);
    }
}
