using RunGroopWebApp.Models;
using System.ComponentModel.DataAnnotations;//bought in for IEnumerable<Club>

namespace RunGroopWebApp.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAll();
        Task<Club> GetByIdAsync(int id);
        Task<Club> GetByIdAsyncNoTracking(int id);//Need to include this so that id is tracked already once for detail page. 
        //In order to eradicate any errors related to tracking, include this method with just a minor change
        Task<IEnumerable<Club>> GetClubByCity(string city);
        //Below are Regular CRUD commands
        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();

    }
}
