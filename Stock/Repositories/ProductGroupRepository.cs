using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Repositories
{
    public class ProductGroupRepository : IProductGroup
    {
        private readonly InventoryContext _context;

        public ProductGroupRepository(InventoryContext context)
        {
            _context = context;
        }
        public PaginatedList<ProductGroup> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize)
        {
            List<ProductGroup> items;

            if(searchText != "" && searchText != null)
            {
                items = _context.ProductGroups
                    .Where(x => x.Name.Contains(searchText) || x.Description.Contains(searchText))
                    .ToList();
            }
            else
            {
                items = _context.ProductGroups.ToList();
            }

            items = DoSort(items, sortProperty, order);

            PaginatedList<ProductGroup> retUnits = new PaginatedList<ProductGroup>(items, pageIndex, pageSize);

            return retUnits;
        }

        public ProductGroup GetItem(Guid id) => _context.ProductGroups.FirstOrDefault(x => x.Id == id);


        public ProductGroup Greate(ProductGroup item)
        {
            _context.ProductGroups.Add(item);
            _context.SaveChanges();
            return item;
        }

        public ProductGroup Edit(ProductGroup item)
        {
            _context.ProductGroups.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

        public ProductGroup Delete(ProductGroup item)
        {
            _context.ProductGroups.Attach(item);
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
            return item;
        }

        public bool IsItemNameExists(string name)
        {
            int ct = _context.ProductGroups.Where(x => x.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemNameExists(string name,Guid id)
        {
            int ct = _context.ProductGroups.Where(x => x.Name.ToLower() == name.ToLower() && x.Id != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


        private List<ProductGroup> DoSort(List<ProductGroup> items, string sortProperty, SortOrder order)
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
