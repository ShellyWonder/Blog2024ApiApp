using Blog2024Api.Models;

namespace Blog2024Api.Data.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int id);
        Task AddTagAsync(string tagText, int postId, string authorId);

        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
        Task RemoveAllTagsByPostIdAsync(int postId);
        bool TagExists(int id);
    }
}
