using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly InventoryContext _context;

        public ProductRepository(InventoryContext context)
        {
            _context = context;
        }
        public PaginatedList<Product> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize)
        {
            List<Product> items;

            if(searchText != "" && searchText != null)
            {
                items = _context.Products
                    .Where(x => x.Name.Contains(searchText) || x.Description.Contains(searchText))
                    .Include(x => x.Unit)
                    .ToList();
            }
            else
            {
                items = _context.Products.Include(x => x.Unit).ToList();
            }

            items = DoSort(items, sortProperty, order);

            PaginatedList<Product> retUnits = new PaginatedList<Product>(items, pageIndex, pageSize);

            return retUnits;
        }

        public Product GetItem(string code) => _context.Products.Include(x => x.Unit).FirstOrDefault(x => x.Code == code);
        public Product GetItem_NoDownload_FG(string code) => _context.Products.FirstOrDefault(x => x.Code == code);


        public Product Greate(Product item)
        {
            _context.Products.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Product Edit(Product item)
        {
            _context.Products.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

        public Product Delete(Product item)
        {
            item = GetItem_NoDownload_FG(item.Code);

            _context.Products.Attach(item);
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
            return item;
        }

        public bool IsItemNameExists(string name)
        {
            int ct = _context.Products.Where(x => x.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemNameExists(string name, string code)
        {
            int ct = _context.Products.Where(x => x.Name.ToLower() == name.ToLower() && x.Code != code).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


        private List<Product> DoSort(List<Product> items, string sortProperty, SortOrder order)
        {
            if (sortProperty.ToLower() == "code")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Code).ToList();
                else
                    items = items.OrderByDescending(x => x.Code).ToList();
            }
            else if(sortProperty.ToLower() == "name")
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
            else if (sortProperty.ToLower() == "cost")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Cost).ToList();
                else
                    items = items.OrderByDescending(x => x.Cost).ToList();
            }
            else if (sortProperty.ToLower() == "price")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Price).ToList();
                else
                    items = items.OrderByDescending(x => x.Price).ToList();
            }
            else if (sortProperty.ToLower() == "unit")
            {
                if (order == SortOrder.Ascending)
                    items = items.OrderBy(x => x.Unit.Name).ToList();
                else
                    items = items.OrderByDescending(x => x.Unit.Name).ToList();
            }

            return items;
        }


    }
}
