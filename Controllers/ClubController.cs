using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;
using System.Diagnostics.Eventing.Reader;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        //private readonly ApplicationDbContext _context; //taken out as the are bought through IClubRepository
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        //private: The field is only accessible within the class or struct where it is declared.
        //readonly: The field can only be assigned a value at the point of declaration or within the constructor(s) of the class. After the constructor has run, the field's value cannot be changed.

        //in controller, to bring in db context, you need to bring it in through the constructor
        public ClubController(/*ApplicationDbContext context, */IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor) 
        {
            //_context = context;
            _clubRepository=clubRepository;
            _photoService=photoService;
            _httpContextAccessor=httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            //List<Club> clubs=_context.Clubs.ToList();
            IEnumerable<Club> clubs =await _clubRepository.GetAll();


            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            //Below line is for data with no address
            //Club club = _context.Clubs.FirstOrDefault(c=>c.Id==id);//indicates when id passed through the argument matched the id of a club when you go through clubs, consider that particular club.
            //in the below line we include address

            //Club club = _context.Clubs.Include(a=>a.Address).FirstOrDefault(c => c.Id == id);

            Club club = await _clubRepository.GetByIdAsync(id);

            //The Include method in Entity Framework(EF) Core helps in eager loading of related data, which means that the related entities are loaded from the database along with the main entity in a single query. 
            //By default, EF Core uses lazy loading, which means related entities are not loaded until you explicitly access the navigation property. Using Include enables eager loading, which fetches the related data as part of the initial query.

            return View(club);

        }
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();//With extensions, you don't even have to ge in dependency injection
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = curUserId,
            };
            return View(createClubViewModel);
        }
        [HttpPost]//to post the request made to create
        public async Task<IActionResult> Create(/*Club club*/ CreateClubViewModel clubVM)
        {
            /*if(!ModelState.IsValid) 
            {
                return View(club);
            }*/ //This is when you have given a link directly for image
            //If you want to upload it to cloudinary service below is the code
            if(ModelState.IsValid) 
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);
                var club=new Club
                { 
                    Title=clubVM.Title,
                    Description= clubVM.Description,
                    Image=result.Url.ToString(),
                    AppUserId=clubVM.AppUserId,//added during dashboard creation
                    Address=new Address
                    {
                        Street=clubVM.Address.Street,
                        City=clubVM.Address.City,
                        State=clubVM.Address.State,
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            //else is excusively for modeState.isValid
            else
            {
                ModelState.AddModelError("", "Photo upload failed");

            }
            //This is for !modelState.isValid
            //_clubRepository.Add(club); 
            //return RedirectToAction("Index");
            return View(clubVM);

        }

        public async Task<IActionResult> Edit(int id) 
        {
            /*If you are tracking 2 database contexes(in here it is id)
             in the same  Controller or method, you need to implement
            other method like GetByIdAsync By no tracking and use it over here*/
            var club = await _clubRepository.GetByIdAsyncNoTracking(id);
            if (club == null) return View("Error");
            //We are pulling club below so that when you open edit page, the data stays filled up in the form
            var clubViewModel = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
            };
            return View(clubViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

            if (userClub == null)
            {
                return View("Error");
            }

            var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(clubVM);
            }

            if (!string.IsNullOrEmpty(userClub.Image))
            {
                _ = _photoService.DeletePhotoAsync(userClub.Image);
            }

            var club = new Club
            {
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = photoResult.Url.ToString(),
                AddressId = clubVM.AddressId,
                Address = clubVM.Address,
            };

            _clubRepository.Update(club);

            return RedirectToAction("Index");
        }


    }
}
