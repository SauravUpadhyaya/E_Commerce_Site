using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication3.Models.Utility;
using X.PagedList;

namespace WebApplication3.Controllers
{   [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? page )
        {
            return View(_db.Products.Include(c => c.ProductType).Include(d => d.SpecialType).ToList().ToPagedList(page??1,6));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Detail(int? id )
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["productTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "Producttype");
            ViewData["SpecialTypeId"] = new SelectList(_db.SpecialTagType.ToList(), "Id", "SpecialTagType");
            var productType = _db.Products.Include(c => c.ProductType).FirstOrDefault(c => c.Id == id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        [HttpPost]
        public IActionResult Index(string searchText)
        {
            if (searchText == null)
            {
                return NotFound();
            }

            var result = (from c in _db.Products
                          where c.Name == searchText
                          select c).FirstOrDefault();
           // if (searchText == _db.Products;
           
              //  int PID = products.Id;
            //   ViewData["productTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "Producttype");
            //  ViewData["SpecialTypeId"] = new SelectList(_db.SpecialTagType.ToList(), "Id", "SpecialTagType");
            //   var ptype = _db.Products.Include(c => c.ProductType).FirstOrDefault(c => c.Id == PID);



            
            //   return View(result);

            if (result == null)
            {
                ViewBag.message = "Sorry, Out of Stock for now!";
              var  p = _db.Products.Include(c => c.ProductType).Include(d => d.SpecialType).ToList();
               
                return Json(new { response = p});
            }

            return Json(new { response = result });
          

        }

        [HttpPost]
        [ActionName("Detail")]
        public IActionResult ProductDetail(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            ViewData["productTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "Producttype");
            ViewData["SpecialTypeId"] = new SelectList(_db.SpecialTagType.ToList(), "Id", "SpecialTagType");
            var productType = _db.Products.Include(c => c.ProductType).FirstOrDefault(c => c.Id == id);
            if (productType == null)
            {
                return NotFound();
            }
             products= HttpContext.Session.Get<List<Products>>("products");
            if(products == null)
            {
                products = new List<Products>();
            }
            products.Add(productType);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }
        //GET Remove action methdo
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //GET product Cart action method

        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            return View(products);
        }


        

    }
}
