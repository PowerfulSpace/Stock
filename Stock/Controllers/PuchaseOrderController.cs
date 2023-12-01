﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public PuchaseOrderController(IPuchaseOrder puchaseOrderRepo)
        {
            _puchaseOrderRepo = puchaseOrderRepo;
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
            return View(poHeader);
        }
    }
}