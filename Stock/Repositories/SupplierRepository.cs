using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Repositories
{
    public class SupplierRepository : ISupplier
    {
        private readonly InventoryContext _context;

        public SupplierRepository(InventoryContext context)
        {
            _context = context;
        }
        public PaginatedList<Supplier> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize)
        {
            List<Supplier> items;

            if(searchText != "" && searchText != null)
            {
                items = _context.Suppliers
                    .Where(x => x.Name.Contains(searchText) || x.Code.Contains(searchText) || x.EmailId.Contains(searchText) || x.Address.Contains(searchText) || x.Phone.Contains(searchText))
                    .ToList();
            }
            else
            {
                items = _context.Suppliers.ToList();
            }

            items = DoSort(items, sortProperty, order);

            PaginatedList<Supplier> retUnits = new PaginatedList<Supplier>(items, pageIndex, pageSize);

            return retUnits;
        }

        public Supplier GetItem(Guid id) => _context.Suppliers.FirstOrDefault(x => x.Id == id);


        public Supplier Greate(Supplier item)
        {
            _context.Suppliers.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Supplier Edit(Supplier item)
        {
            _context.Suppliers.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

        public Supplier Delete(Supplier item)
        {
            _context.Suppliers.Attach(item);
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
            return item;
        }

        public bool IsItemNameExists(string name)
        {
            int ct = _context.Suppliers.Where(x => x.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemNameExists(string name,Guid id)
        {
            int ct = _context.Suppliers.Where(x => x.Name.ToLower() == name.ToLower() && x.Id != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemCodeExists(string itemCode)
        {
            int ct = _context.Suppliers.Where(n => n.Code.ToLower() == itemCode.ToLower()).Count();

            if (ct > 0) return true;
            else return false;
        }

        public bool IsItemCodeExists(string itemCode, Guid id)
        {
            int ct = _context.Suppliers.Where(n => n.Code.ToLower() == itemCode.ToLower() && n.Id != id).Count();

            if (ct > 0) return true;
            else return false;
        }

        public bool IsItemEmailExists(string email)
        {
            int ct = _context.Suppliers.Where(n => n.EmailId.ToLower() == email.ToLower()).Count();

            if (ct > 0) return true;
            else return false;
        }

        public bool IsItemEmailExists(string email, Guid id)
        {
            int ct = _context.Suppliers.Where(n => n.EmailId.ToLower() == email.ToLower() && n.Id != id).Count();

            if (ct > 0) return true;
            else return false;
        }




        private List<Supplier> DoSort(List<Supplier> items, string sortProperty, SortOrder order)
        {
            if (sortProperty.ToLower() == "code")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Code).ToList();
                else
                    items = items.OrderByDescending(x => x.Code).ToList();
            }
            else if (sortProperty.ToLower() == "name")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Name).ToList();
                else
                    items = items.OrderByDescending(x => x.Name).ToList();
            }
            else if (sortProperty.ToLower() == "emailId")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.EmailId).ToList();
                else
                    items = items.OrderByDescending(x => x.EmailId).ToList();
            }
            else if (sortProperty.ToLower() == "address")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Address).ToList();
                else
                    items = items.OrderByDescending(x => x.Address).ToList();
            }
            else if (sortProperty.ToLower() == "phone")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Phone).ToList();
                else
                    items = items.OrderByDescending(x => x.Phone).ToList();
            }

            return items;
        }


    }
}
