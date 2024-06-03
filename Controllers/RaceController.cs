using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.Services;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(/*ApplicationDbContext context*/IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor) 
        {
            //_context=context;
            _raceRepository=raceRepository;
            _photoService=photoService;
            _httpContextAccessor=httpContextAccessor;

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAllRaces();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            //Race race=_context.Races.Include(a=>a.Address).FirstOrDefault(r => r.Id == id);
            Race race = await _raceRepository.GetRaceByIdAsync(id);
            return View(race);
        }
        public IActionResult Create()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel
            {
                AppUserId = curUserID
            };
            return View(createRaceViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(/*Race race*/CreateRaceViewModel raceVM) 
        {
            //if(!ModelState.IsValid) //If model state is not valid=> proper related data types for the model attributes are not given
            //{
            //    return View(race);//Remain in the form page
            //}
            //_raceRepository.Add(race);//Adding to database
            //return RedirectToAction("Index");//If model is valid=> Redirect to Index page

            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId=raceVM.AppUserId,//added during Dashboard
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                    }
                };
                _raceRepository.Add(race);
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
            return View(raceVM);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetRaceByIdAsync(id);
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View(raceVM);
            }

            var userRace = await _raceRepository.GetRaceByIdAsyncNoTracking(id);

            if (userRace == null)
            {
                return View("Error");
            }

            var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(raceVM);
            }

            if (!string.IsNullOrEmpty(userRace.Image))
            {
                _ = _photoService.DeletePhotoAsync(userRace.Image);
            }

            var race = new Race
            {
                Id = id,
                Title = raceVM.Title,
                Description = raceVM.Description,
                Image = photoResult.Url.ToString(),
                AddressId = raceVM.AddressId,
                Address = raceVM.Address,
            };

            _raceRepository.Update(race);

            return RedirectToAction("Index");
        }

    }
}
