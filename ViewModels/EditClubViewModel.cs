using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;
//Related to Edit page
namespace RunGroopWebApp.ViewModels
{
    public class EditClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }//exclsively for image uploads
        public string? URL {  get; set; }//needs to have a question mark for sure
        public int? AddressId {  get; set; }//needs to have ? for sure for edit page to work.
        public Address Address {  get; set; }
        public ClubCategory ClubCategory { get; set; }

    }
}
