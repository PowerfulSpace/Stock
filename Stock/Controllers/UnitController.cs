using Microsoft.AspNetCore.Mvc;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Controllers
{
    public class UnitController : Controller
    {

        private readonly IUnit _unitRepo;

        public UnitController(IUnit context)
        {
            _unitRepo = context;
        }

        public IActionResult Index()
        {
            var units = _unitRepo.GetUnits();
            return View(units);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var unit = new Unit();
            return View(unit);
        }

        [HttpPost]
        public IActionResult Create(Unit unit)
        {
            try
            {
                unit = _unitRepo.Greate(unit);
            }
            catch { }
           
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var unit = _unitRepo.GetUnit(id);

            if (unit != null)
            {
                return View(unit);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var unit = _unitRepo.GetUnit(id);

            if (unit != null)
            {
                return View(unit);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Unit unit)
        {
            try
            {
                unit = _unitRepo.Edit(unit);
            }
            catch { }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var unit = _unitRepo.GetUnit(id);

            if (unit != null)
            {
                return View(unit);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(Unit unit)
        {
            try
            {
                unit = _unitRepo.Delete(unit);
            }
            catch { }

            return RedirectToAction(nameof(Index));
        }

    }
}
