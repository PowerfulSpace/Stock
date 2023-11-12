using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Interfaces;
using Stock.Models;
using Stock.Services.Pagination;
using Stock.Services.Sorting;

namespace Stock.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {

        private readonly ISupplier _supplierRepo;

        public SupplierController(ISupplier context)
        {
            _supplierRepo = context;
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsEmailExists(string EmailId, Guid id = default(Guid))
        {
            bool isexists = _supplierRepo.IsItemEmailExists(EmailId, id);

            if (isexists)
                return Json(data: false);
            else
                return Json(data: true);
        }

        public IActionResult Index(string sortExpression="", string searchText = "",int currentPage = 1, int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("code");
            sortModel.AddColumn("emailId");
            sortModel.AddColumn("address");
            sortModel.AddColumn("phone");
            sortModel.ApplySort(sortExpression);

            var suppliers = _supplierRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, currentPage, pageSize);

            var pager = new PagerModel(suppliers.TotalRecords, currentPage, pageSize);
            pager.SortExpression = sortExpression;

            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = searchText;
            ViewBag.Pager = pager;
            TempData["CurrentPage"] = currentPage;

            return View(suppliers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var supplier = new Supplier();
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Create(Supplier supplier)
        {
            var bolret = false;
            string errMessage = "";

            try
            {

                if (_supplierRepo.IsItemNameExists(supplier.Name) == true)
                    errMessage = errMessage + " " + " Supplier Name " + supplier.Name + " Exists Already";

                if (_supplierRepo.IsItemCodeExists(supplier.Code) == true)
                    errMessage = errMessage + " " + " Supplier Code " + supplier.Code + " Exists Already";

                if (_supplierRepo.IsItemEmailExists(supplier.EmailId) == true)
                    errMessage = errMessage + " " + " Supplier Email " + supplier.EmailId + " Exists Already";

                if (errMessage == "")
                {
                    supplier = _supplierRepo.Greate(supplier);
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
                return View(supplier);
            }
            else
            {
                TempData["SuccessMessage"] = "Supplier " + supplier.Name + " Created Successfully";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Supplier supplier = _supplierRepo.GetItem(id);
            TempData.Keep("CurrentPage");
            if (supplier != null)
            {
                return View(supplier);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var supplier = _supplierRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (supplier != null)
            {
                return View(supplier);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Supplier supplier)
        {
            var bolret = false;
            string errMessage = "";

            try
            {

                if (_supplierRepo.IsItemNameExists(supplier.Name,supplier.Id) == true)
                    errMessage = errMessage + " " + " Supplier Name " + supplier.Name + " Exists Already";

                if (_supplierRepo.IsItemCodeExists(supplier.Code, supplier.Id) == true)
                    errMessage = errMessage + " " + " Supplier Code " + supplier.Code + " Exists Already";

                if (_supplierRepo.IsItemEmailExists(supplier.EmailId, supplier.Id) == true)
                    errMessage = errMessage + " " + " Supplier Email " + supplier.EmailId + " Exists Already";

                if (errMessage == "")
                {
                    supplier = _supplierRepo.Edit(supplier);
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
                return View(supplier);
            }
            else
            {
                TempData["SuccessMessage"] = "Supplier " + supplier.Name + " Saved Successfully";
                return RedirectToAction(nameof(Index), new { currentPage = currentPage });
            }
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var supplier = _supplierRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (supplier != null)
            {
                return View(supplier);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(Supplier supplier)
        {
            try
            {
                supplier = _supplierRepo.Delete(supplier);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(supplier);
            }



            var currentPage = 1;
            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"]!;
            }

            TempData["SuccessMessage"] = "Supplier " + supplier.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { currentPage = currentPage });
        }

    }
}
