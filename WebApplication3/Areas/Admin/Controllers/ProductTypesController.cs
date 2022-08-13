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

    public class ProductTypesController : Controller
    {
        private ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_db.ProductType.ToList());
        }

        //GET Create Action Method
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //POST Create Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType Ptype)
        {
            
            if (ModelState.IsValid)
            {
                _db.ProductType.Add(Ptype);
                //_db.Add(Producttype);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product type has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(Ptype);
        }
        //Get Delete Action Method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
                     
            }
            var protype = _db.ProductType.Find(id);
            if (protype == null)
            {
                return NotFound();
            }


            return View(protype);
        } 

        //Post Delete Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, ProductType prtype)
        {
            if (id == null)
            {
                return NotFound();

            }
            var protype = _db.ProductType.Find(id);
            if (protype == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.ProductType.Remove(protype);
                 await _db.SaveChangesAsync();
                TempData["delete"] = "Product type has been deleted";
                return RedirectToAction(nameof(Index));

            }
            return View(prtype);
        }



        // Get Edit
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var protype1 = _db.ProductType.Find(id);
            if (protype1 == null)
            {
                return NotFound();
            }


            return View(protype1);
        }

        //Post Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, ProductType prtype)
        {
            if (ModelState.IsValid)
            {
                _db.ProductType.Update(prtype);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product type has been Updated";
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

            var productType = _db.ProductType.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }




    }
}