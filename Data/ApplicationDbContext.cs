using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Data
{
    //Represents entity framework-data layer
    //public class ApplicationDbContext : DbContext//Removed after getting in Identity Framework
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) //learn the line byheart
        {

        }

        public DbSet<Club> Clubs { get; set; }//here the once inside<> represent thatparticular class
        public DbSet<Race> Races { get; set; }
        public DbSet<Address> Addresses { get; set; }

    }
}
