using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Models;

namespace Stock.Controllers
{
    public class UnitController : Controller
    {

        private readonly InventoryContext _context;

        public UnitController(InventoryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var units = _context.Units.ToList();
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
                _context.Units.Add(unit);
                _context.SaveChanges();
            }
            catch { }
           
            return RedirectToAction(nameof(Index));
        }

        private Unit GetUnit(Guid id) => _context.Units.FirstOrDefault(x => x.Id == id);

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var unit = GetUnit(id);

            if (unit != null)
            {
                return View(unit);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var unit = GetUnit(id);

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
                _context.Units.Attach(unit);
                _context.Entry(unit).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch { }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var unit = GetUnit(id);

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
                _context.Units.Attach(unit);
                _context.Entry(unit).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            catch { }

            return RedirectToAction(nameof(Index));
        }

    }
}
