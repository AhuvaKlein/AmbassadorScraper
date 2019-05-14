using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AUScraper.Web.Models;
using AUScraper.Api;

namespace AUScraper.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult ViewResults(string query)
        {
            IEnumerable<Product> products = AUApi.GetProducts(query);
            return View(products);
        }
    }
}
