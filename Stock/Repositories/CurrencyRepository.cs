using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Repositories
{
    public class CurrencyRepository : ICurrency
    {
        private readonly InventoryContext _context;
        private string _errors = string.Empty;

        public CurrencyRepository(InventoryContext context)
        {
            _context = context;
        }
        public PaginatedList<Currency> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize)
        {
            List<Currency> items;

            if (searchText != "" && searchText != null)
            {
                items = _context.Currencies
                    .Where(x => x.Name.Contains(searchText) || x.Description.Contains(searchText))
                    .ToList();
            }
            else
            {
                items = _context.Currencies.ToList();
            }

            items = DoSort(items, sortProperty, order);

            PaginatedList<Currency> retUnits = new PaginatedList<Currency>(items, pageIndex, pageSize);

            return retUnits;
        }

        public Currency GetItem(Guid id) => _context.Currencies.FirstOrDefault(x => x.Id == id);


        public bool Greate(Currency item)
        {
            try
            {
                _context.Currencies.Add(item);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _errors = "Create failed - Sql Exception Occured, Error info: " + e.Message;
                return false;
            }
           
        }

        public bool Edit(Currency item)
        {
            try
            {
                _context.Currencies.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _errors = "Edit Failed - Sql Exception Occured, Error info: " + e.Message;
                return false;
            }
        }

        public bool Delete(Currency item)
        {
            try
            {
                _context.Currencies.Attach(item);
                _context.Entry(item).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _errors = "Delete failed - Sql Exception Occured, Error info: " + e.Message;
                return false;
            }

        }

        public bool IsItemNameExists(string name)
        {
            int ct = _context.Currencies.Where(x => x.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemNameExists(string name, Guid id)
        {
            int ct = _context.Currencies.Where(x => x.Name.ToLower() == name.ToLower() && x.Id != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsCurrencyCombExists(Guid srcIsCurrencyId, Guid excCurrencyId)
        {
            int ct = _context.Currencies.Where(x => x.Id == srcIsCurrencyId && x.ExchangeCurrencyId == excCurrencyId).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public string GetErrors()
        {
            return _errors;
        }

        private List<Currency> DoSort(List<Currency> items, string sortProperty, SortOrder order)
        {
            if (sortProperty.ToLower() == "name")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Name).ToList();
                else
                    items = items.OrderByDescending(x => x.Name).ToList();
            }
            else if (sortProperty.ToLower() == "description")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Description).ToList();
                else
                    items = items.OrderByDescending(x => x.Description).ToList();
            }

            return items;
        }
    }
}
