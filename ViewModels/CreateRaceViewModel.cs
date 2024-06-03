using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.ViewModels
{
    public class CreateRaceViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }//Here having IFormFile for image is compulsary when you are uploading an image to some bucket service
        public RaceCategory RaceCategory { get; set; }
        public string AppUserId { get; set; }
    }
}
