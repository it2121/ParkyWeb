using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;



        public HomeController(ILogger<HomeController> logger ,
            INationalParkRepository  npRepo,
            ITrailRepository trailRepo)
        {
            _logger= logger;
            _npRepo = npRepo;
            _trailRepo=trailRepo;
        }



        public async Task<IActionResult> Index()
        {

            IndexVM listOfParkAndTrails = new IndexVM()
            {
                TrailList = await _trailRepo.GetAllAsync(SD.TrailApiPath),
                NationalParkList = await _npRepo.GetAllAsync(SD.NpAPIPAth)

            };


            return View("~/Pages/Index.cshtml", listOfParkAndTrails);
        }
    }
}
