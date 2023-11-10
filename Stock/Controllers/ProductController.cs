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
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHost;

        private readonly IProduct _productRepo;
        private readonly IUnit _unitRepo;
        private readonly IBrand _brandRepo;
        private readonly ICategory _categoryRepo;
        private readonly IProductGroup _productGroupRepo;
        private readonly IProductProfile _productProfileRepo;

        public ProductController(
            IProduct context,
            IUnit unitRepo,
            IBrand brandRepo,
            ICategory categoryRepo,
            IProductGroup productGroupRepo,
            IProductProfile productProfileRepo,
            IWebHostEnvironment webHost)
        {
            _productRepo = context;
            _unitRepo = unitRepo;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
            _productGroupRepo = productGroupRepo;
            _productProfileRepo = productProfileRepo;
            _webHost = webHost;
        }

        public IActionResult Index(string sortExpression="", string searchText = "",int currentPage = 1, int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("code");
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.AddColumn("cost");
            sortModel.AddColumn("price");
            sortModel.AddColumn("unit");
            sortModel.ApplySort(sortExpression);

            var products = _productRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, currentPage, pageSize);

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
            var product = new Product();
            ViewBag.Units = GetUnits();
            ViewBag.Brands = GetBrands();
            ViewBag.Categories = GetCategories();
            ViewBag.ProductGroups = GetProductGroups();
            ViewBag.ProductProfiles = GetProductProfiles();
            return View(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            var bolret = false;
            string errMessage = "";

            try
            {

                if (product.Description.Length < 4 || product.Description == null)
                    errMessage = "Product Description Must be atleast 4 Characters";

                if (_productRepo.IsItemNameExists(product.Name) == true)
                    errMessage = errMessage + " " + " Product Name " + product.Name + " Exists Already";

                if(errMessage == "")
                {

                    string uniqueFileName = GetUploadedFileName(product);
                    if(uniqueFileName != null)
                        product.PhotoUrl = uniqueFileName;


                    product = _productRepo.Greate(product);
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
                return View(product);
            }
            else
            {
                TempData["SuccessMessage"] = "Product " + product.Name + " Created Successfully";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Details(string code)
        {
            Product product = _productRepo.GetItem(code);
            TempData.Keep("CurrentPage");
            if (product != null)
            {
                return View(product);
            }

            return NotFound();
        }


        [HttpGet]
        public IActionResult Edit(string code)
        {
            var product = _productRepo.GetItem(code);
            ViewBag.Units = GetUnits();
            ViewBag.Brands = GetBrands();
            ViewBag.Categories = GetCategories();
            ViewBag.ProductGroups = GetProductGroups();
            ViewBag.ProductProfiles = GetProductProfiles();
            TempData.Keep("CurrentPage");

            if (product != null)
            {
                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var bolret = false;
            string errMessage = "";

            try
            {
                if (product.Description.Length < 4 || product.Description == null)
                    errMessage = "Product Description Must be atleast 4 Characters";

                if (_productRepo.IsItemNameExists(product.Name,product.Code) == true)
                    errMessage = errMessage + " " + " Product Name " + product.Name + " Exists Already";

                if (errMessage == "")
                {
                    product = _productRepo.Edit(product);
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
                return View(product);
            }
            else
            {
                TempData["SuccessMessage"] = "Product " + product.Name + " Saved Successfully";
                return RedirectToAction(nameof(Index), new { currentPage = currentPage });
            }
        }
        [HttpGet]
        public IActionResult Delete(string code)
        {
            var product = _productRepo.GetItem(code);
            TempData.Keep("CurrentPage");

            if (product != null)
            {
                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            try
            {
                product = _productRepo.Delete(product);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(product);
            }



            var currentPage = 1;
            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"]!;
            }

            TempData["SuccessMessage"] = "Product " + product.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { currentPage = currentPage });
        }


        private List<SelectListItem> GetUnits()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<Unit> items = _unitRepo.GetItems("name", SortOrder.Ascending, "", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Text = "---Select Unit---",
                Value = ""
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }

        private List<SelectListItem> GetBrands()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<Brand> items = _brandRepo.GetItems("name", SortOrder.Ascending, "", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Text = "---Select Brand---",
                Value = ""
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }
        private List<SelectListItem> GetCategories()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<Category> items = _categoryRepo.GetItems("name", SortOrder.Ascending, "", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Text = "---Select Category---",
                Value = ""
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }
        private List<SelectListItem> GetProductGroups()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<ProductGroup> items = _productGroupRepo.GetItems("name", SortOrder.Ascending, "", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Text = "---Select ProductGroup---",
                Value = ""
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }
        private List<SelectListItem> GetProductProfiles()
        {
            List<SelectListItem> listIItems = new List<SelectListItem>();

            PaginatedList<ProductProfile> items = _productProfileRepo.GetItems("name", SortOrder.Ascending, "", 1, 1000);

            listIItems = items.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            SelectListItem defItem = new SelectListItem()
            {
                Text = "---Select ProductProfile---",
                Value = ""
            };

            listIItems.Insert(0, defItem);
            return listIItems;
        }

        private string GetUploadedFileName(Product product)
        {
            string uniqueFileName = string.Empty;

            if(product.ProductPhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ProductPhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.ProductPhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
