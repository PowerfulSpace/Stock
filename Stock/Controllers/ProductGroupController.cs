using Microsoft.AspNetCore.Mvc;
using Stock.Interfaces;
using Stock.Models;
using Stock.Services.Pagination;
using Stock.Services.Sorting;

namespace Stock.Controllers
{
    public class ProductGroupController : Controller
    {

        private readonly IProductGroup _productGroupRepo;

        public ProductGroupController(IProductGroup context)
        {
            _productGroupRepo = context;
        }

        public IActionResult Index(string sortExpression="", string searchText = "",int currentPage = 1, int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);

            var units = _productGroupRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, currentPage, pageSize);

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
            var productGroup = new ProductGroup();
            return View(productGroup);
        }

        [HttpPost]
        public IActionResult Create(ProductGroup productGroup)
        {
            var bolret = false;
            string errMessage = "";

            try
            {

                if (productGroup.Description.Length < 4 || productGroup.Description == null)
                    errMessage = "ProductGroup Description Must be atleast 4 Characters";

                if (_productGroupRepo.IsItemNameExists(productGroup.Name) == true)
                    errMessage = errMessage + " " + " ProductGroup Name " + productGroup.Name + " Exists Already";

                if(errMessage == "")
                {
                    productGroup = _productGroupRepo.Greate(productGroup);
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
                return View(productGroup);
            }
            else
            {
                TempData["SuccessMessage"] = "ProductGroup " + productGroup.Name + " Created Successfully";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            ProductGroup productGroup = _productGroupRepo.GetItem(id);
            TempData.Keep("CurrentPage");
            if (productGroup != null)
            {
                return View(productGroup);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var productGroup = _productGroupRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (productGroup != null)
            {
                return View(productGroup);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(ProductGroup productGroup)
        {
            var bolret = false;
            string errMessage = "";

            try
            {
                if (productGroup.Description.Length < 4 || productGroup.Description == null)
                    errMessage = "ProductGroup Description Must be atleast 4 Characters";

                if (_productGroupRepo.IsItemNameExists(productGroup.Name,productGroup.Id) == true)
                    errMessage = errMessage + " " + " ProductGroup Name " + productGroup.Name + " Exists Already";

                if (errMessage == "")
                {
                    productGroup = _productGroupRepo.Edit(productGroup);
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
                return View(productGroup);
            }
            else
            {
                TempData["SuccessMessage"] = "ProductGroup " + productGroup.Name + " Saved Successfully";
                return RedirectToAction(nameof(Index), new { currentPage = currentPage });
            }
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var productGroup = _productGroupRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (productGroup != null)
            {
                return View(productGroup);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(ProductGroup productGroup)
        {
            try
            {
                productGroup = _productGroupRepo.Delete(productGroup);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(productGroup);
            }



            var currentPage = 1;
            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"]!;
            }

            TempData["SuccessMessage"] = "ProductGroup " + productGroup.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { currentPage = currentPage });
        }

    }
}
