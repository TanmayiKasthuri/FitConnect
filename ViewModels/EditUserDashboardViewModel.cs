namespace RunGroopWebApp.ViewModels
{
    public class EditUserDashboardViewModel
    {
        public string id {  get; set; }
        public int? Pace { get; set; }
        public int? Milage { get; set; }
        public string? ProfileImageUrl { get; set; }//For storing image in cloudinary and getting string - URL of where it has stayed.
        public string? City { get; set; }
        public string? State { get; set; }
        public IFormFile? Image { get; set; }
    }
}
