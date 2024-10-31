using Blog2024Api.Models;

namespace Blog2024Api.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(int id);
        Task CreateCommentAsync(Comment comment);
        Task<Comment?>GetExistingCommentAsync(int id);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
        Task<bool> CommentExistsAsync(int id);
    }
}
