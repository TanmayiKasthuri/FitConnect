using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroopWebApp.Models
{
    
    public class AppUser:IdentityUser
    {
        //Having an id and declaring primary key for it are steps needed to create a db
        //Once you bring in Identity Framework into this, you need not need those
        //[Key]
        //public string Id { get; set; }
        public int? Pace { get; set; }

        public int? Milage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        [ForeignKey("Address")]
        public int? AddressId { get; set; }/*This comes with question mark because- we use AppUser during resgistration
                                            and during registration Address is not compulsary*/
        public Address? Address { get; set; }

        public ICollection<Club> Clubs { get; set; }
        public ICollection<Race> Races { get; set; }
        


    }
}
