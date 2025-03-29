using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

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
            HttpContext.Session.SetString("JWToken", objUser.Token);



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


            return RedirectToAction("Login", "Home");
           // return View("~/Views/Home/Login.cshtml");
        }

        public IActionResult Logout() {

            HttpContext.Session.SetString("JWToken","");
            return RedirectToAction("Index", "Home");
         //   return View("~/Pages/Index.cshtml");
        
        
        }
    }
}
