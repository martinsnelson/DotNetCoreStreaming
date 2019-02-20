using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DoNetCoreStreaming.Controllers
{
    public class ContaController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult NaoAutorizado()
        {
            return View();
        }

        public IActionResult Proibido()
        {
            return View();
        }
    }
}