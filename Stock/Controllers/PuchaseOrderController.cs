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

        public PuchaseOrderController(IPuchaseOrder puchaseOrderRepo, IProduct productRepo)
        {
            _puchaseOrderRepo = puchaseOrderRepo;
            _productRepo = productRepo;
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

            poHeader.PoDetails.Add(new PoDetail() { Id = Guid.NewGuid() });

            PopulateViewBags();

            return View(poHeader);
        }

        [HttpPost]
        public IActionResult Create(PoHeader poHeader)
        {
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

                //PopulateViewBags();

                return View(poHeader);
            }
            else
            {
                TempData["SuccessMessage"] = "PoHeader " + poHeader.PoNumber + " Created Successfully";

                return RedirectToAction(nameof(Index));
            }
        }




        private void PopulateViewBags()
        {
            ViewBag.Products = GetProducts();
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

    }
}
