﻿using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllRaces();
        Task<Race> GetRaceByIdAsync(int id);
        Task<Race> GetRaceByIdAsyncNoTracking(int id);
        Task<IEnumerable<Race>> GetRacesByCity(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
        
    }

}
