namespace Blog2024ApiApp.Data.Repositories.Interfaces
{
    public interface ISlugRepository
    {
        public bool IsSlugUnique(string slug);
    }
}
