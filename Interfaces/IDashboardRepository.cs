using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Race>> GetAllRaces();
        Task<List<Club>> GetAllClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetUserByIdNoTracking(string id);
        public bool Update(AppUser user);
        public bool Save();
    }
}
