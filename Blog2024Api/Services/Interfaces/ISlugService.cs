﻿namespace Blog2024Api.Services.Interfaces
{
    public interface ISlugService
    {
        public string UrlFriendly(string title);
        public bool IsUnique(string slug);
    }
}
