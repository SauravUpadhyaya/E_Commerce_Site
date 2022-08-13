using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using WebApplication3.Data;
using WebApplication3.Areas.Admin.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;

        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }

        public IActionResult Index()
        {
             return View(_db.Products.Include(c => c.ProductType).Include(d => d.SpecialType));
            //return View();
        }

        //   [HttpPost("{lowAmount}/{largeAmount}")]

        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductType).Include(c => c.SpecialType)
                .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
           if (lowAmount == null || largeAmount == null)
         {
           products = _db.Products.Include(c => c.ProductType).Include(c => c.SpecialType).ToList();
        }
            // return View(products);
           // return new JsonResult(products);
              return Json(new
             {
              aaData = products
            //  sEcho = param.sEcho,
            //  iTotalDisplayRecords = totalCount,
            //   iTotalRecords = totalCount
              }
             );
            //  , JsonRequestBehavior.AllowGet);

        }
        //   return Json(products);


        //[HttpPost("{lowAmount}/{largeAmount}")]
        public JsonResult Filter(DataTablesParam param, decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductType).Include(c => c.SpecialType)
                            .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();


            if (param.sSearch != null)
            {
                 products = _db.Products.Include(c => c.ProductType).Include(c => c.SpecialType)
                           .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            }
                if (lowAmount == null || largeAmount == null)
            {
                products = _db.Products.Include(c => c.ProductType).Include(c => c.SpecialType).ToList();
            }
            // return View(products);
            // return new JsonResult(products);
            return Json(new
            {
                aaData = products,
                  sEcho = param.sEcho,
                //  iTotalDisplayRecords = totalCount,
                //   iTotalRecords = totalCount
            }
           );
            //  , JsonRequestBehavior.AllowGet);
        }
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "Producttype");
            ViewData["SpecialTypeId"] = new SelectList(_db.SpecialTagType.ToList(), "Id", "SpecialTagType");

            return View();
        }




                //POST Create Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products, IFormFile image)
        {

            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == products.Name);
                if (searchProduct != null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["productTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "Producttype");
                    ViewData["SpecialTypeId"] = new SelectList(_db.SpecialTagType.ToList(), "Id", "SpecialTagType");
                    return View(products);
                }

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    products.Image = "Images/noimage.PNG";
                }


                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(products);
        }


        // Get Edit

        public ActionResult Edit(int? id)
        {

            ViewData["productTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "Producttype");
            ViewData["SpecialTypeId"] = new SelectList(_db.SpecialTagType.ToList(), "Id", "SpecialTagType");
            if (id == null)
            {
                return NotFound();

            }
            var protype1 = _db.Products.Include(c => c.ProductType).Include(d => d.SpecialType).FirstOrDefault(c => c.Id == id);
            if (protype1 == null)
            {
                return NotFound();
            }


            return View(protype1);
        }

        //Post Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    products.Image = "Images/noimage.PNG";
                }

                _db.Products.Update(products);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product type has been Updated";
                return RedirectToAction(nameof(Index));

            }
            return View(products);
        }

        //GET Details Action Method

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["productTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "Producttype");
            ViewData["SpecialTypeId"] = new SelectList(_db.SpecialTagType.ToList(), "Id", "SpecialTagType");
            var productType = _db.Products.Include(c => c.ProductType).Include(d => d.SpecialType).FirstOrDefault(c => c.Id == id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //Get Delete Action Method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            ViewData["productTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "Producttype");
            ViewData["SpecialTypeId"] = new SelectList(_db.SpecialTagType.ToList(), "Id", "SpecialTagType");
            var productType = _db.Products.Include(c => c.ProductType).Include(d => d.SpecialType).FirstOrDefault(c => c.Id == id);
            if (productType == null)
            {
                return NotFound();
            }


            return View(productType);
        }




        //Post Delete Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, Products prtype)
        {
            if (id == null)
            {
                return NotFound();

            }
            var produ = _db.Products.FirstOrDefault(c => c.Id == id);
            if (produ == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Products.Remove(produ);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Product has been deleted";
                return RedirectToAction(nameof(Index));

            }
            return View(prtype);
        }
    }
}