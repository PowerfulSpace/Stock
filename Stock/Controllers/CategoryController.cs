using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Interfaces;
using Stock.Models;
using Stock.Services.Pagination;
using Stock.Services.Sorting;

namespace Stock.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {

        private readonly ICategory _categoryRepo;

        public CategoryController(ICategory context)
        {
            _categoryRepo = context;
        }

        public IActionResult Index(string sortExpression="", string searchText = "",int currentPage = 1, int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);

            var units = _categoryRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, currentPage, pageSize);

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
            var category = new Category();
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            var bolret = false;
            string errMessage = "";

            try
            {

                if (category.Description.Length < 4 || category.Description == null)
                    errMessage = "Category Description Must be atleast 4 Characters";

                if (_categoryRepo.IsItemNameExists(category.Name) == true)
                    errMessage = errMessage + " " + " Category Name " + category.Name + " Exists Already";

                if(errMessage == "")
                {
                    category = _categoryRepo.Greate(category);
                    bolret = true;
                }
 
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            if(bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(category);
            }
            else
            {
                TempData["SuccessMessage"] = "Category " + category.Name + " Created Successfully";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Category category = _categoryRepo.GetItem(id);
            TempData.Keep("CurrentPage");
            if (category != null)
            {
                return View(category);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var category = _categoryRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (category != null)
            {
                return View(category);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            var bolret = false;
            string errMessage = "";

            try
            {
                if (category.Description.Length < 4 || category.Description == null)
                    errMessage = "Category Description Must be atleast 4 Characters";

                if (_categoryRepo.IsItemNameExists(category.Name,category.Id) == true)
                    errMessage = errMessage + " " + " Category Name " + category.Name + " Exists Already";

                if (errMessage == "")
                {
                    category = _categoryRepo.Edit(category);
                    bolret = true;
                } 
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            var currentPage = 1;
            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"]!;
            }
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(category);
            }
            else
            {
                TempData["SuccessMessage"] = "Category " + category.Name + " Saved Successfully";
                return RedirectToAction(nameof(Index), new { currentPage = currentPage });
            }
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var category = _categoryRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (category != null)
            {
                return View(category);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            try
            {
                category = _categoryRepo.Delete(category);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(category);
            }



            var currentPage = 1;
            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"]!;
            }

            TempData["SuccessMessage"] = "Category " + category.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { currentPage = currentPage });
        }

    }
}
