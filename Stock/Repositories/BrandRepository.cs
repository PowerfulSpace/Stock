using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Repositories
{
    public class BrandRepository : IBrand
    {
        private readonly InventoryContext _context;

        public BrandRepository(InventoryContext context)
        {
            _context = context;
        }
        public PaginatedList<Brand> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize)
        {
            List<Brand> items;

            if(searchText != "" && searchText != null)
            {
                items = _context.Brands
                    .Where(x => x.Name.Contains(searchText) || x.Description.Contains(searchText))
                    .ToList();
            }
            else
            {
                items = _context.Brands.ToList();
            }

            items = DoSort(items, sortProperty, order);

            PaginatedList<Brand> retUnits = new PaginatedList<Brand>(items, pageIndex, pageSize);

            return retUnits;
        }

        public Brand GetItem(Guid id) => _context.Brands.FirstOrDefault(x => x.Id == id);


        public Brand Greate(Brand item)
        {
            _context.Brands.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Brand Edit(Brand item)
        {
            _context.Brands.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

        public Brand Delete(Brand item)
        {
            _context.Brands.Attach(item);
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
            return item;
        }

        public bool IsItemNameExists(string name)
        {
            int ct = _context.Brands.Where(x => x.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemNameExists(string name,Guid id)
        {
            int ct = _context.Brands.Where(x => x.Name.ToLower() == name.ToLower() && x.Id != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


        private List<Brand> DoSort(List<Brand> items, string sortProperty, SortOrder order)
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
