using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class AccountController : Controller
    {

        //Manager is a part of Identity frameworks that gives you different built in functionality to work with
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        //What constructor is doing below is dependency injection
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        //If you donot specify anything on top of a method it would automatically imply to be [HttpGet]
        public IActionResult Login()
        {
            /*When you type in some password or some other field and reload the page
             For you to fall back into login retaining whatever you have typed, This response would be useful*/
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel LoginVM)
        {
            if(!ModelState.IsValid)
            {
                return View(LoginVM);//Here we are returning the login page if state is not valid
                //You need not put any new error messages as we have already included them in our model with [Required("Error Message")]
            }

            var user = await _userManager.FindByEmailAsync(LoginVM.EmailAddress);
            //The if block below+ further if block is best case scenario i.e; when the user could sign-in
            if (user != null) 
            {
                var passwordCheck=await _userManager.CheckPasswordAsync(user, LoginVM.Password);
                if(passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, LoginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }
                //If you comeout of the above loop, It automatically indicates failure as you are forepassing return
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(LoginVM);
            }
            //User Not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(LoginVM);





        }
        public IActionResult Register()
        {
            /*When you type in some password or some other field and reload the page
             For you to fall back into login retaining whatever you have typed, This response would be useful*/
            var response = new RegisterViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel RegisterVM)
        {
            if(!ModelState.IsValid) 
            { 
                return View(RegisterVM);
            }
            var user = await _userManager.FindByEmailAsync(RegisterVM.EmailAddress);
            if(user != null) 
            {
                TempData["Error"] = "This email address already exists";
                return View(RegisterVM);
            }
            var newUser = new AppUser()
            {
                Email = RegisterVM.EmailAddress,
                //UserName is not something that resides in AppUser but is bought in from Identity User
                UserName=RegisterVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, RegisterVM.Password);
            if (newUserResponse.Succeeded) 
            {
                await _userManager.AddToRoleAsync(newUser,UserRoles.User);

            }
            return RedirectToAction("Index", "Race");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Race");
        }
        
    }
}
