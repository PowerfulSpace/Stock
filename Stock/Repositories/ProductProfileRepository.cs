using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Repositories
{
    public class ProductProfileRepository : IProductProfile
    {
        private readonly InventoryContext _context;

        public ProductProfileRepository(InventoryContext context)
        {
            _context = context;
        }
        public PaginatedList<ProductProfile> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize)
        {
            List<ProductProfile> items;

            if(searchText != "" && searchText != null)
            {
                items = _context.ProductProfiles
                    .Where(x => x.Name.Contains(searchText) || x.Description.Contains(searchText))
                    .ToList();
            }
            else
            {
                items = _context.ProductProfiles.ToList();
            }

            items = DoSort(items, sortProperty, order);

            PaginatedList<ProductProfile> retUnits = new PaginatedList<ProductProfile>(items, pageIndex, pageSize);

            return retUnits;
        }

        public ProductProfile GetItem(Guid id) => _context.ProductProfiles.FirstOrDefault(x => x.Id == id);


        public ProductProfile Greate(ProductProfile item)
        {
            _context.ProductProfiles.Add(item);
            _context.SaveChanges();
            return item;
        }

        public ProductProfile Edit(ProductProfile item)
        {
            _context.ProductProfiles.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

        public ProductProfile Delete(ProductProfile item)
        {
            _context.ProductProfiles.Attach(item);
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
            return item;
        }

        public bool IsItemNameExists(string name)
        {
            int ct = _context.ProductProfiles.Where(x => x.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemNameExists(string name,Guid id)
        {
            int ct = _context.ProductProfiles.Where(x => x.Name.ToLower() == name.ToLower() && x.Id != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


        private List<ProductProfile> DoSort(List<ProductProfile> items, string sortProperty, SortOrder order)
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
