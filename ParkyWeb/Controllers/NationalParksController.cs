﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;


        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }

        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }


        [Authorize(Roles ="Admin")]
        public async  Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark();

            if (id == null)
            {
                return View(obj);

            }
            obj = await _npRepo.GetAsync(SD.NpAPIPAth, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (obj == null)
            {
                return NotFound();
            }
            else
            {

                return View(obj);

            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {


                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream()) { 
                        fs1.CopyTo(ms1);    
                        p1=ms1.ToArray();
                        }
                    }

                    obj.Picture=p1;

                }
                else
                {
                    var objFromDb = await _npRepo.GetAsync(SD.NpAPIPAth, obj.Id, HttpContext.Session.GetString("JWToken"));

                    obj.Picture = objFromDb.Picture;
                }

                if (obj.Id == 0)
                {
                    await _npRepo.CreateAsync(SD.NpAPIPAth, obj, HttpContext.Session.GetString("JWToken"));

                }
                else
                {
                    await _npRepo.UpdateAsync(SD.NpAPIPAth+obj.Id , obj, HttpContext.Session.GetString("JWToken"));



                }

                return RedirectToAction(nameof(Index));
            }

            else
            {
                return View(obj);



            }
        }


        public async Task<IActionResult> GetAllNationalPark()
        {


            return Json(new { data = await _npRepo.GetAllAsync(SD.NpAPIPAth, HttpContext.Session.GetString("JWToken")) } );
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {

            var status = await _npRepo.DeleteAsync(SD.NpAPIPAth, id, HttpContext.Session.GetString("JWToken"));

            if(status)
            {
                return Json(new { success = true, message = "Delete Successful" });


            }
            return Json(new { success = false, message = "Delete not Successful" });

        }
    }  
}
