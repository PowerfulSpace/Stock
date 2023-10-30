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

        public IActionResult Index(string sortExpression="")
        {
            ViewData["SortParamName"] = "name";
            ViewData["SortParamDescription"] = "description";

            ViewData["SortIconName"] = "";
            ViewData["SortIconDescription"] = "";


            SortOrder sortOrder ;
            string sortProperty;

            switch (sortExpression.ToLower())
            {
                case "name_desc":
                    sortOrder = SortOrder.Descending;
                    sortProperty = "name";
                    ViewData["SortParamName"] = "name";
                    ViewData["SortIconName"] = "bi bi-chevron-compact-up";
                    break;
                case "description":
                    sortOrder = SortOrder.Ascending;
                    sortProperty = "description";
                    ViewData["SortParamDescription"] = "description_desc";
                    ViewData["SortIconDescription"] = "bi bi-chevron-compact-down";
                    break;
                case "description_desc":
                    sortOrder = SortOrder.Descending;
                    sortProperty = "description";
                    ViewData["SortParamDescription"] = "description";
                    ViewData["SortIconDescription"] = "bi bi-chevron-compact-up";
                    break;
                default:
                    sortOrder = SortOrder.Ascending;
                    sortProperty = "name";
                    ViewData["SortParamName"] = "name_desc";
                    ViewData["SortIconName"] = "bi bi-chevron-compact-down";
                    break;
            }


            var units = _unitRepo.GetUnits(sortProperty, sortOrder);
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
