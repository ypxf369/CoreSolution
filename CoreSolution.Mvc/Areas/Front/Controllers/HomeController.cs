using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreSolution.Mvc.Areas.Front.Controllers
{
    [Area("Front")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}