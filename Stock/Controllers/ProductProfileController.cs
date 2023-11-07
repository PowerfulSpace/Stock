using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Interfaces;
using Stock.Models;
using Stock.Services.Pagination;
using Stock.Services.Sorting;

namespace Stock.Controllers
{
    [Authorize]
    public class ProductProfileController : Controller
    {

        private readonly IProductProfile _ProductProfileRepo;

        public ProductProfileController(IProductProfile context)
        {
            _ProductProfileRepo = context;
        }

        public IActionResult Index(string sortExpression="", string searchText = "",int currentPage = 1, int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);

            var productProfiles = _ProductProfileRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, currentPage, pageSize);

            var pager = new PagerModel(productProfiles.TotalRecords, currentPage, pageSize);
            pager.SortExpression = sortExpression;

            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = searchText;
            ViewBag.Pager = pager;
            TempData["CurrentPage"] = currentPage;

            return View(productProfiles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var productProfile = new ProductProfile();
            return View(productProfile);
        }

        [HttpPost]
        public IActionResult Create(ProductProfile productProfile)
        {
            var bolret = false;
            string errMessage = "";

            try
            {

                if (productProfile.Description.Length < 4 || productProfile.Description == null)
                    errMessage = "ProductProfile Description Must be atleast 4 Characters";

                if (_ProductProfileRepo.IsItemNameExists(productProfile.Name) == true)
                    errMessage = errMessage + " " + " ProductProfile Name " + productProfile.Name + " Exists Already";

                if(errMessage == "")
                {
                    productProfile = _ProductProfileRepo.Greate(productProfile);
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
                return View(productProfile);
            }
            else
            {
                TempData["SuccessMessage"] = "ProductProfile " + productProfile.Name + " Created Successfully";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            ProductProfile productProfile = _ProductProfileRepo.GetItem(id);
            TempData.Keep("CurrentPage");
            if (productProfile != null)
            {
                return View(productProfile);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var productProfile = _ProductProfileRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (productProfile != null)
            {
                return View(productProfile);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(ProductProfile productProfile)
        {
            var bolret = false;
            string errMessage = "";

            try
            {
                if (productProfile.Description.Length < 4 || productProfile.Description == null)
                    errMessage = "ProductProfile Description Must be atleast 4 Characters";

                if (_ProductProfileRepo.IsItemNameExists(productProfile.Name,productProfile.Id) == true)
                    errMessage = errMessage + " " + " ProductProfile Name " + productProfile.Name + " Exists Already";

                if (errMessage == "")
                {
                    productProfile = _ProductProfileRepo.Edit(productProfile);
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
                return View(productProfile);
            }
            else
            {
                TempData["SuccessMessage"] = "ProductProfile " + productProfile.Name + " Saved Successfully";
                return RedirectToAction(nameof(Index), new { currentPage = currentPage });
            }
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var productProfile = _ProductProfileRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (productProfile != null)
            {
                return View(productProfile);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(ProductProfile productProfile)
        {
            try
            {
                productProfile = _ProductProfileRepo.Delete(productProfile);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(productProfile);
            }



            var currentPage = 1;
            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"]!;
            }

            TempData["SuccessMessage"] = "ProductProfile " + productProfile.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { currentPage = currentPage });
        }

    }
}
