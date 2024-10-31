namespace Blog2024Api.Data.Repositories.Interfaces
{
    public interface ISlugRepository
    {
        public bool IsSlugUnique(string slug);
    }
}
