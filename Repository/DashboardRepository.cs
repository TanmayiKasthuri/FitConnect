using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        {
            _context=context;
            _httpContextAccessor=httpContextAccessor;
        }
        public async Task<List<Club>> GetAllClubs()
        {
            //curUser=>Current User
            //The HttpContext property of IHttpContextAccessor provides access to the current HTTP context, which includes details about the current HTTP request, response, user, session, etc.
            var curUser =_httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(r => r.AppUser.Id == curUser.ToString());
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllRaces()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = _context.Races.Where(r => r.AppUser.Id == curUser.ToString());
            return userRaces.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByIdNoTracking(string id)
        {
            return await _context.Users.Where(u=>u.Id==id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }
        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved>0?true:false;
        }
    }
}
