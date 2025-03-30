using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System.Security.Claims;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IAccountRepository _accRepo;



        public HomeController(ILogger<HomeController> logger ,
            INationalParkRepository  npRepo,
            ITrailRepository trailRepo, IAccountRepository accrRepo)
        {
            _logger= logger;
            _npRepo = npRepo;
            _trailRepo=trailRepo;
            _accRepo=accrRepo;
        }



        public async Task<IActionResult> Index()
        {
            string temp = HttpContext.Session.GetString("JWToken");

            IndexVM listOfParkAndTrails = new IndexVM()
            {
                TrailList = await _trailRepo.GetAllAsync(SD.TrailApiPath,HttpContext.Session.GetString("JWToken")),
                NationalParkList = await _npRepo.GetAllAsync(SD.NpAPIPAth, HttpContext.Session.GetString("JWToken"))

            };


            return View("~/Pages/Index.cshtml", listOfParkAndTrails);
        }


        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User ();

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {                                                             
            User objUser = await _accRepo.LoginAsync(SD.AccApiPath + "authenticate/", obj);
            if(objUser.Token == null)
            {
                return View();

            }


            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim (ClaimTypes.Name ,objUser.Username));
            identity.AddClaim(new Claim (ClaimTypes.Role ,objUser.Role));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            HttpContext.Session.SetString("JWToken", objUser.Token);


            TempData["alert"] = "Welcome " +objUser.Username;
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]

        public IActionResult Register()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accRepo.RegisterAsync(SD.AccApiPath + "register/", obj);
            if (result == false)
            {
                return View();

            }

            TempData["alert"] = "Registeration Sccucessful ";

            return RedirectToAction("Login", "Home");
           // return View("~/Views/Home/Login.cshtml");
        }

        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken","");
            return RedirectToAction("Index", "Home");
         //   return View("~/Pages/Index.cshtml");
        
        
        }




        [HttpGet]
        public IActionResult AccessDenied()
        {

            return View();
        }

    }
}
