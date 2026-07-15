namespace MiniBlog.Application.DTOs
{
    public record PostDto(
        int Id,
        string Title,
        string Content,
        DateTime CreatedAt,
        string AuthorName,
        int CommentsCount);

    public record CreatePostDto(string Title, string Content);

    public record CommentDto(
        int Id,
        string Text,
        DateTime CreatedAt,
        string AuthorName);

    public record CreateCommentDto(string Text);
}
