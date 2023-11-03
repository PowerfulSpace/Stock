using Microsoft.AspNetCore.Mvc;
using Stock.Interfaces;
using Stock.Models;
using Stock.Services.Pagination;
using Stock.Services.Sorting;

namespace Stock.Controllers
{
    public class UnitController : Controller
    {

        private readonly IUnit _unitRepo;

        public UnitController(IUnit context)
        {
            _unitRepo = context;
        }

        public IActionResult Index(string sortExpression="", string searchText = "",int currentPage = 1, int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);

            var units = _unitRepo.GetUnits(sortModel.SortedProperty, sortModel.SortedOrder, searchText,currentPage,pageSize);

            var pager = new PagerModel(units.TotalRecords, currentPage, pageSize);
            pager.SortExpression = sortExpression;


            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = searchText;
            ViewBag.Pager = pager;
            TempData["CurrentPage"] = currentPage;

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

            TempData["SuccessMessage"] = "Unit " + unit.Name + " Created Successfully";

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

            TempData["SuccessMessage"] = "Unit " + unit.Name + " Saved Successfully";

            var currentPage = 1;
            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"]!;
            }


            return RedirectToAction(nameof(Index),new { currentPage = currentPage });
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

            TempData["SuccessMessage"] = "Unit " + unit.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }

    }
}
