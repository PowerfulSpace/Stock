using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Repositories
{
    public class PuchaseOrderRepository : IPuchaseOrder
    {
        private readonly InventoryContext _context;
        private string _errors = string.Empty;

        public PuchaseOrderRepository(InventoryContext context)
        {
            _context = context;
        }
       

        public PaginatedList<PoHeader> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize)
        {

            List<PoHeader> items;

            if (searchText != "" && searchText != null)
            {
                items = _context.PoHeaders
                    .Include(x => x.Supplier)
                    .Include(x => x.BaseCurrency)
                    .Include(x => x.PoCurrency)
                    .Where(x => x.PoNumber.Contains(searchText) || x.QuotationNo.Contains(searchText))
                        .ToList();
            }
            else
            {
                items = _context.PoHeaders
                    .Include(x => x.Supplier)
                    .Include(x => x.BaseCurrency)
                    .Include(x => x.PoCurrency)
                        .ToList();
            }

            items = DoSort(items, sortProperty, order);

            PaginatedList<PoHeader> retUnits = new PaginatedList<PoHeader>(items, pageIndex, pageSize);

            return retUnits;
        }

        public PoHeader GetItem(Guid id)
        {
            var item = _context.PoHeaders
                .Where(x => x.Id == id)
                .Include(x => x.PoDetails)
                    .FirstOrDefault();


            return item;
        }




        public bool Greate(PoHeader item)
        {
            bool retVal = false;
            _errors = string.Empty;

            try
            {
                _context.PoHeaders.Add(item);
                _context.SaveChanges();
                retVal = true;
            }
            catch (Exception e)
            {
                _errors = "Create failed - Sql Exception Occured, Error info: " + e.Message;
            }

            return retVal;
        }

        public bool Edit(PoHeader item)
        {
            bool retVal = false;
            _errors = string.Empty;

            try
            {
                _context.PoHeaders.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                retVal = true;
            }
            catch (Exception e)
            {
                _errors = "Edit Failed - Sql Exception Occured, Error info: " + e.Message;
            }

            return retVal;
        }

        public bool Delete(PoHeader item)
        {
            bool retVal = false;
            _errors = string.Empty;

            try
            {
                _context.PoHeaders.Attach(item);
                _context.Entry(item).State = EntityState.Deleted;
                _context.SaveChanges();
                retVal = true;
            }
            catch (Exception e)
            {
                _errors = "Delete failed - Sql Exception Occured, Error info: " + e.Message;
            }

            return retVal;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public bool IsPoNumberExists(string poNumber)
        {
            int ct = _context.PoHeaders.Where(x => x.PoNumber.ToLower() == poNumber.ToLower()).Count();
            if (ct > 0)
            {
                _errors = " PoHeader Name " + poNumber + " Exists Already";
                return true;
            }
            else
                return false;
        }

        public bool IsPoNumberExists(string poNumber, Guid id = default)
        {
            if (id == default)
                return IsPoNumberExists(poNumber);

            var poHeads = _context.PoHeaders.Where(x => x.PoNumber.ToLower() == poNumber.ToLower()).Max(x =>x.Id);

            if (poHeads == default || poHeads == id)
            {
                return false;
            }
            else
                return true;
        }


        public bool IsQuotationNoExists(string quoNumber)
        {
            int ct = _context.PoHeaders.Where(x => x.QuotationNo.ToLower() == quoNumber.ToLower()).Count();
            if (ct > 0)
            {
                _errors = " PoHeader Name " + quoNumber + " Exists Already";
                return true;
            }
            else
                return false;
        }

        public bool IsQuotationNoExists(string quoNumber, Guid id = default)
        {
            if (id == default)
                return IsQuotationNoExists(quoNumber);

            var strQuotMo = _context.PoHeaders.Where(x => x.QuotationNo.ToLower() == quoNumber.ToLower()).Max(x => x.Id);

            if (strQuotMo == default || strQuotMo == id)
            {
                return false;
            }
            else
                return true;

        }




        private List<PoHeader> DoSort(List<PoHeader> items, string sortProperty, SortOrder order)
        {
            if (sortProperty.ToLower() == "ponumber")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.PoNumber).ToList();
                else
                    items = items.OrderByDescending(x => x.PoNumber).ToList();
            }
            else if (sortProperty.ToLower() == "quotationno")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.QuotationNo).ToList();
                else
                    items = items.OrderByDescending(x => x.QuotationNo).ToList();
            }
            else if (sortProperty.ToLower() == "podate")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.PoDate).ToList();
                else
                    items = items.OrderByDescending(x => x.PoDate).ToList();
            }

            return items;
        }
    }
}
