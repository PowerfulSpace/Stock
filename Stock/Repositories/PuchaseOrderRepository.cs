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
                    .Where(x => x.PoNumber.Contains(searchText) || x.QuotationNo.Contains(searchText))
                    .Include(x => x.Supplier)
                    .Include(x => x.BaseCurrency)
                    .Include(x => x.PoCurrency)                  
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
            PoHeader item = _context.PoHeaders.Where(i => i.Id == id)
                     .Include(d => d.PoDetails)
                     .ThenInclude(i => i.Product)
                     .ThenInclude(u => u.Unit)
                     .FirstOrDefault()!;

            item.PoDetails.ForEach(i => i.UnitName = i.Product.Unit.Name);
            item.PoDetails.ForEach(p => p.Description = p.Product.Description);
            item.PoDetails.ForEach(p => p.Total = p.Quantity * p.PrcInBaseCur);

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

        public bool Edit(PoHeader poHeader)
        {
            bool retVal = false;
            _errors = "";

            try
            {

                List<PoDetail> poDetails = _context.PoDetails.Where(d => d.Id == poHeader.Id).ToList();
                _context.PoDetails.RemoveRange(poDetails);
                _context.SaveChanges();

                _context.Attach(poHeader);
                _context.Entry(poHeader).State = EntityState.Modified;
                _context.PoDetails.AddRange(poHeader.PoDetails);
                _context.SaveChanges();


                retVal = true;
            }
            catch (Exception ex)
            {
                _errors = "Update Failed - Sql Exception Occured , Error Info : " + ex.Message;
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

        public string GetNewPoNumber()
        {
            string ponumber = "";
            var LastPoNumber = _context.PoHeaders.Max(x => x.PoNumber);

            if(LastPoNumber == null)
            {
                ponumber = "PO00001";
            }
            else
            {
                int lastdigit = 1;
                int.TryParse(LastPoNumber.Substring(2, 5).ToString(), out lastdigit);

                ponumber = "PO" + (lastdigit + 1).ToString().PadLeft(5, '0');
            }

            return ponumber;
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
