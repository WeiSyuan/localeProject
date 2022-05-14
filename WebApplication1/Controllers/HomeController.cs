using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.ApiModels;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CallApiService _callApiService;

        public HomeController(ILogger<HomeController> logger, CallApiService callApiService)
        {
            _logger = logger;
            this._callApiService = callApiService;
        }

        public IActionResult Index()
        {
            var fruits = new List<FruitModels>() {
                 new FruitModels(){Name="蓮霧",Count=99 },
                 new FruitModels(){Name="鳳梨",Count=88 },
                 new FruitModels(){Name="西瓜",Count=77 }
            };
            return View(fruits);
        }

        public async Task<IActionResult> Privacy()
        {
            await _callApiService.CallSelfApi();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
