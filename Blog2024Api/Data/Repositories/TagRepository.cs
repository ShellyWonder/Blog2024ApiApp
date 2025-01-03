﻿using Microsoft.EntityFrameworkCore;
using Blog2024Api.Data.Repositories.Interfaces;
using Blog2024Api.Data;
using Blog2024Api.Models;

namespace Blog2024Api.Data.Repositories
{
    public class TagRepository(ApplicationDbContext context) : ITagRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.Include(t => t.Author)
                                      .Include(t => t.Post)
                                      .ToListAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _context.Tags.Include(t => t.Author)
                                      .Include(t => t.Post)
                                      .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddTagAsync(string tagText, int postId, string authorId)
        {
            // Check if the tag already exists in the database for the same post
            var existingTag = await _context.Tags
                                            .FirstOrDefaultAsync(t => t.Text == tagText && t.PostId == postId);

            if (existingTag == null)
            {
                // If the tag doesn't exist, create a new one
                var newTag = new Tag
                {
                    Text = tagText,
                    PostId = postId,
                    // Associate the tag with the author (ApplicationUserId)
                    AuthorId = authorId
                };

                _context.Tags.Add(newTag);
                await _context.SaveChangesAsync(); // Save the tag to the database
            }
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync(); ;
        }

        public async Task DeleteTagAsync(int id)
        {

            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAllTagsByPostIdAsync(int postId)
        {
            var tagsToRemove = await _context.Tags
                                             .Where(t => t.PostId == postId)
                                             .ToListAsync();

            if (tagsToRemove.Any())
            {
                _context.Tags.RemoveRange(tagsToRemove);
                await _context.SaveChangesAsync();
            }
        }
        public bool TagExists(int id)
        {
            return _context.Tags.Any(t => t.Id == id);
        }

    }
}
