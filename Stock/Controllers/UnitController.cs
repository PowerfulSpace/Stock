﻿using Microsoft.AspNetCore.Mvc;

namespace Stock.Controllers
{
    public class UnitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
