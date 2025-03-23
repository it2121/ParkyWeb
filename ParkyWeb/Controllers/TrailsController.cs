using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using ParkyWeb;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepo;


        public TrailsController(ITrailRepository trailRepo)
        {
            _trailRepo = trailRepo;
        }

        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task<IActionResult> GetAllTrails()
        {


            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailApiPath) });
        }

    }
}
