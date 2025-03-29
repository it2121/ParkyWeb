using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using ParkyWeb;
using ParkyWeb.Models.ViewModel;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepo;
        private readonly INationalParkRepository _npRepo;


        public TrailsController(ITrailRepository trailRepo, INationalParkRepository npRepo)
        {
            _trailRepo = trailRepo;
            _npRepo = npRepo;
        }

        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> noList = await _npRepo.GetAllAsync(SD.NpAPIPAth, HttpContext.Session.GetString("JWToken"));
            TrailsVM objVM = new TrailsVM()
            {

                NationalParkList = noList.Select(i=> new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { 
                
                Text= i.Name,
                Value = i.Id.ToString()
                }),
                Trail = new Trail() 

            };


            if (id == null)
            {
                return View(objVM);

            }  
            objVM.Trail = await _trailRepo.GetAsync(SD.TrailApiPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (objVM.Trail == null)
            {
                return NotFound();
            }
            else
            {

                return View(objVM);

            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (this.ModelState.IsValid)
            {



                if (obj.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailApiPath, obj.Trail, HttpContext.Session.GetString("JWToken"));

                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailApiPath + obj.Trail.Id, obj.Trail, HttpContext.Session.GetString("JWToken"));



                }

                return RedirectToAction(nameof(Index));
            }

            else
            {
                IEnumerable<NationalPark> noList = await _npRepo.GetAllAsync(SD.NpAPIPAth, HttpContext.Session.GetString("JWToken"));
                TrailsVM objVM = new TrailsVM()
                {

                    NationalParkList = noList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {

                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail

                };

                return View(objVM);



            }
        } 


        public async Task<IActionResult> GetAllTrail()
        {


            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailApiPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete] 
        public async Task<IActionResult> Delete(int id)
        {

            var status = await _trailRepo.DeleteAsync(SD.TrailApiPath, id, HttpContext.Session.GetString("JWToken"));

            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });


            }
            return Json(new { success = false, message = "Delete not Successful" });

        }

    }
}
