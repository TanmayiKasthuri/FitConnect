using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    //IClubRepository is the interface that is being implemented here
    //Since you are implementing that here, once you bring that in, all the methods of that will be inherited here
    public class ClubRepository : IClubRepository//Inheritng IClubRepository to implement it here.
    {
        private readonly ApplicationDbContext _context;

        public ClubRepository(ApplicationDbContext context)//with ApplicationDbContext we are getting in the database table.
        {
            _context = context;
        }
        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            //Add, Delete operations were changes made to db whereas get operation is to returen result. So you return your result in this portion of code
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _context.Clubs.Include(a=>a.Address).FirstOrDefaultAsync(i=>i.Id==id);
        }

        public async Task<Club> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Clubs.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id); ;
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(c=>c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved>0?true:false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
