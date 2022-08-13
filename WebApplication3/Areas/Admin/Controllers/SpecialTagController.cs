using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using WebApplication3.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SpecialTagController : Controller
    {
        private ApplicationDbContext _db;

        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            return View(_db.SpecialTagType.ToList());
        }

        //GET Create Action Method

        public ActionResult Create()
        {
            return View();
        }

        //POST Create Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialType Stype)
        {

            if (ModelState.IsValid)
            {
                _db.SpecialTagType.Add(Stype);
                //_db.Add(Producttype);
                await _db.SaveChangesAsync();
                TempData["save"] = "Special Tag type has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(Stype);
        }
        //Get Delete Action Method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var protype = _db.SpecialTagType.Find(id);
            if (protype == null)
            {
                return NotFound();
            }


            return View(protype);
        }

        //Post Delete Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, SpecialType Sttype)
        {
            if (id == null)
            {
                return NotFound();

            }
            var Statype = _db.SpecialTagType.Find(id);
            if (Statype == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.SpecialTagType.Remove(Statype);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Special Tage type has been deleted";
                return RedirectToAction(nameof(Index));

            }
            return View(Sttype);
        }



        // Get Edit

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var protype1 = _db.SpecialTagType.Find(id);
            if (protype1 == null)
            {
                return NotFound();
            }


            return View(protype1);
        }

        //Post Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, SpecialType prtype)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTagType.Update(prtype);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Special Tag type has been Updated";
                return RedirectToAction(nameof(Index));

            }
            return View(prtype);
        }

        //GET Details Action Method

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialType = _db.SpecialTagType.Find(id);
            if (specialType == null)
            {
                return NotFound();
            }
            return View(specialType);
        }




    }
}