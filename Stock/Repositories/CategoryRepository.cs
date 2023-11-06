using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Repositories
{
    public class CategoryRepository : ICategory
    {
        private readonly InventoryContext _context;

        public CategoryRepository(InventoryContext context)
        {
            _context = context;
        }
        public PaginatedList<Category> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize)
        {
            List<Category> items;

            if(searchText != "" && searchText != null)
            {
                items = _context.Categories
                    .Where(x => x.Name.Contains(searchText) || x.Description.Contains(searchText))
                    .ToList();
            }
            else
            {
                items = _context.Categories.ToList();
            }

            items = DoSort(items, sortProperty, order);

            PaginatedList<Category> retUnits = new PaginatedList<Category>(items, pageIndex, pageSize);

            return retUnits;
        }

        public Category GetItem(Guid id) => _context.Categories.FirstOrDefault(x => x.Id == id);


        public Category Greate(Category item)
        {
            _context.Categories.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Category Edit(Category item)
        {
            _context.Categories.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

        public Category Delete(Category item)
        {
            _context.Categories.Attach(item);
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
            return item;
        }

        public bool IsItemNameExists(string name)
        {
            int ct = _context.Categories.Where(x => x.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemNameExists(string name,Guid id)
        {
            int ct = _context.Categories.Where(x => x.Name.ToLower() == name.ToLower() && x.Id != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


        private List<Category> DoSort(List<Category> items, string sortProperty, SortOrder order)
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
