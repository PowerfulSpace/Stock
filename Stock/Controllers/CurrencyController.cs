using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Stock.Interfaces;
using Stock.Models;
using Stock.Services.Pagination;
using Stock.Services.Sorting;

namespace Stock.Controllers
{
    [Authorize]
    public class CurrencyController : Controller
    {

        private readonly ICurrency _currencyRepo;

        public CurrencyController(ICurrency context)
        {
            _currencyRepo = context;
        }

        public IActionResult Index(string sortExpression="", string searchText = "",int currentPage = 1, int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);

            var currenties = _currencyRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, currentPage, pageSize);

            var pager = new PagerModel(currenties.TotalRecords, currentPage, pageSize);
            pager.SortExpression = sortExpression;

            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = searchText;
            ViewBag.Pager = pager;
            TempData["CurrentPage"] = currentPage;

            return View(currenties);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var currency = new Currency();

            PopulateViewBags();

            return View(currency);
        }

        [HttpPost]
        public IActionResult Create(Currency currency)
        {
            var bolret = false;
            string errMessage = "";

            try
            {
                bolret = _currencyRepo.Greate(currency);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            if(bolret == false)
            {
                errMessage = errMessage + " " + _currencyRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);

                PopulateViewBags();

                return View(currency);
            }
            else
            {
                TempData["SuccessMessage"] = "Currency " + currency.Name + " Created Successfully";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Currency currency = _currencyRepo.GetItem(id);
            TempData.Keep("CurrentPage");
            if (currency != null)
            {
                return View(currency);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var currency = _currencyRepo.GetItem(id);
            PopulateViewBags();
            TempData.Keep("CurrentPage");

            if (currency != null)
            {
                return View(currency);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Currency currency)
        {
            var bolret = false;
            string errMessage = "";

            try
            {
                bolret = _currencyRepo.Edit(currency);
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
                errMessage = errMessage + " " + _currencyRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);

                PopulateViewBags();

                return View(currency);
            }
            else
            {
                TempData["SuccessMessage"] = "Currency " + currency.Name + " Saved Successfully";
                return RedirectToAction(nameof(Index), new { currentPage = currentPage });
            }
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var currency = _currencyRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (currency != null)
            {
                return View(currency);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(Currency currency)
        {
            var bolret = false;
            string errMessage = "";
            try
            {
                bolret = _currencyRepo.Delete(currency);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(currency);
            }

            var currentPage = 1;

            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"]!;
            }

            if (bolret == false)
            {
                errMessage = errMessage + " " + _currencyRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(currency);
            }
            else
            {
                TempData["SuccessMessage"] = "Currency " + currency.Name + " Deleted Successfully";
                return RedirectToAction(nameof(Index), new { currentPage = currentPage });
            }
        }


        private void PopulateViewBags()
        {
            ViewBag.ExchangeCurrencyId = GetCurrencies();
        }


        private List<SelectListItem> GetCurrencies()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<Currency> items = _currencyRepo.GetItems("name", SortOrder.Ascending, "", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Text = "---Select Currency---",
                Value = ""
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }

    }
}
