﻿using Blog2024ApiApp.Models;

namespace Blog2024ApiApp.Services.Interfaces
{
    public interface ITagService
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
