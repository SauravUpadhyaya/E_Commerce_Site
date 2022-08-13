using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Models.Utility;

namespace WebApplication3.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;


        public OrderController(ApplicationDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        //GET Checkout actioin method

        public IActionResult Checkout()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Invoice(Order orders, OrderDetails p, IFormFile images)
        {
            string Emailbody = "";
            string body = "Thank You";
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            
            //var message = new Message(new string[] { "gemsresort@gmail.com" }, "Test email", "This is the content from our email.");
            var msg = new Message( new string[]

                                    {orders.Email}, "Purchase Product:     "+"  " + "   Delivery DateTime: " + orders.OrderDate,
                                    Emailbody = body + "   " + orders.Name + "," + "  for shopping with us. Hope you will have a great day !!!                                                                               " + "                                                                                                                                                                                                                                                                              Regards:Saurav Upadhyaya" 
            
                                 );
            _emailSender.SendEmail(msg);
            return View();
        }

        //POST Checkout action method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.PorductId = product.Id;
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }

            anOrder.OrderNo = GetOrderNo();
            _db.Order.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Products>());
            return View();
        }


        public string GetOrderNo()
        {
            int rowCount = _db.Order.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
    }
}
