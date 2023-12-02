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
    public class PuchaseOrderController : Controller
    {
        private readonly IPuchaseOrder _puchaseOrderRepo;
        private readonly IProduct _productRepo;
        private readonly ISupplier _supplierRepo;
        private readonly ICurrency _currencyRepo;

        public PuchaseOrderController(
            IPuchaseOrder puchaseOrderRepo,
            IProduct productRepo,
            ISupplier supplierRepo,
            ICurrency currencyRepo)
        {
            _puchaseOrderRepo = puchaseOrderRepo;
            _productRepo = productRepo;
            _supplierRepo = supplierRepo;
            _currencyRepo = currencyRepo;
        }


        public IActionResult Index(string sortExpression = "", string searchText = "", int currentPage = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("ponumber");
            sortModel.AddColumn("quotationno");
            sortModel.AddColumn("podate");

            sortModel.ApplySort(sortExpression);

            var products = _puchaseOrderRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, currentPage, pageSize);

            var pager = new PagerModel(products.TotalRecords, currentPage, pageSize);
            pager.SortExpression = sortExpression;

            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = searchText;
            ViewBag.Pager = pager;
            TempData["CurrentPage"] = currentPage;

            return View(products);
        }




        [HttpGet]
        public IActionResult Create()
        {
            var poHeader = new PoHeader();

            poHeader.PoDetails.Add(new PoDetail() { Id = Guid.NewGuid(),  });

            PopulateViewBags();

            return View(poHeader);
        }

        [HttpPost]
        public IActionResult Create(PoHeader poHeader)
        {
            poHeader.PoDetails.RemoveAll(x => x.Quantity == 0);

            var bolret = false;
            string errMessage = "";

            try
            {
                bolret = _puchaseOrderRepo.Greate(poHeader);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            if (bolret == false)
            {
                errMessage = errMessage + " " + _puchaseOrderRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);

                PopulateViewBags();

                return View(poHeader);
            }
            else
            {
                TempData["SuccessMessage"] = "PoHeader " + poHeader.PoNumber + " Created Successfully";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            PoHeader poHeader = _puchaseOrderRepo.GetItem(id);

            PopulateViewBags();

            TempData.Keep("CurrentPage");
            if (poHeader != null)
            {
                return View(poHeader);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {

            var item = _puchaseOrderRepo.GetItem(id);
            TempData.Keep("CurrentPage");

            if (item != null)
            {
                return View(item);
            }

            return NotFound();
        }



        private void PopulateViewBags()
        {
            ViewBag.Products = GetProducts();
            ViewBag.Suppliers = GetSuppliers();
            ViewBag.PoCurrencies = GetPoCurrencies();
            ViewBag.BaseCurrencies = GetBaseCurrencies();
        }


        private List<SelectListItem> GetProducts()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<Product> items = _productRepo.GetItems("name", SortOrder.Ascending, "", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Value = x.Code.ToString(),
                Text = x.Name
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Value = "",
                Text = "---Select Product---"
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }

        private List<SelectListItem> GetSuppliers()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<Supplier> items = _supplierRepo.GetItems("name", SortOrder.Ascending, "", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Value = "",
                Text = "---Select Supplier---"
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }

        private List<SelectListItem> GetPoCurrencies()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<Currency> items = _currencyRepo.GetItems("name", SortOrder.Ascending, "", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Value = "",
                Text = "---Select Po Currency---"
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }

        private List<SelectListItem> GetBaseCurrencies()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<Currency> items = _currencyRepo.GetItems("name", SortOrder.Ascending, "USD", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Value = "",
                Text = "---Select Base Currency---"
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }

    }
}
