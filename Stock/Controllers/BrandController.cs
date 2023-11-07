using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Interfaces;
using Stock.Models;
using Stock.Services.Pagination;
using Stock.Services.Sorting;

namespace Stock.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {

        private readonly IBrand _brandRepo;

        public BrandController(IBrand context)
        {
            _brandRepo = context;
        }

        public IActionResult Index(string sortExpression="", string searchText = "",int currentPage = 1, int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);

            var brands = _brandRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, currentPage, pageSize);

            var pager = new PagerModel(brands.TotalRecords, currentPage, pageSize);
            pager.SortExpression = sortExpression;

            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = searchText;
            ViewBag.Pager = pager;
            TempData["CurrentPage"] = currentPage;

            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var brand = new Brand();
            return View(brand);
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            var bolret = false;
            string errMessage = "";

            try
            {

                if (brand.Description.Length < 4 || brand.Description == null)
                    errMessage = "Brand Description Must be atleast 4 Characters";

                if (_brandRepo.IsItemNameExists(brand.Name) == true)
                    errMessage = errMessage + " " + " Brand Name " + brand.Name + " Exists Already";

                if(errMessage == "")
                {
                    brand = _brandRepo.Greate(brand);
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
                return View(brand);
            }
            else
            {
                TempData["SuccessMessage"] = "Brand " + brand.Name + " Created Successfully";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _brandRepo.GetItem(id);
            TempData.Keep("CurrentPage");
            if (brand != null)
            {
                return View(brand);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var brand = _brandRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (brand != null)
            {
                return View(brand);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            var bolret = false;
            string errMessage = "";

            try
            {
                if (brand.Description.Length < 4 || brand.Description == null)
                    errMessage = "Brand Description Must be atleast 4 Characters";

                if (_brandRepo.IsItemNameExists(brand.Name,brand.Id) == true)
                    errMessage = errMessage + " " + " Brand Name " + brand.Name + " Exists Already";

                if (errMessage == "")
                {
                    brand = _brandRepo.Edit(brand);
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
                return View(brand);
            }
            else
            {
                TempData["SuccessMessage"] = "Brand " + brand.Name + " Saved Successfully";
                return RedirectToAction(nameof(Index), new { currentPage = currentPage });
            }
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var brand = _brandRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (brand != null)
            {
                return View(brand);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            try
            {
                brand = _brandRepo.Delete(brand);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(brand);
            }



            var currentPage = 1;
            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"]!;
            }

            TempData["SuccessMessage"] = "Brand " + brand.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { currentPage = currentPage });
        }

    }
}
