using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context) 
        { 
            _context=context;
        }
        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAllRaces()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race> GetRaceByIdAsync(int id)
        {
            return await _context.Races.Include(a=>a.Address).FirstOrDefaultAsync(r=>r.Id==id);
        }

        public async Task<Race> GetRaceByIdAsyncNoTracking(int id)
        {
            return await _context.Races.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Race>> GetRacesByCity(string city)
        {
            return await _context.Races.Where(c=>c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved>0?true:false;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}
