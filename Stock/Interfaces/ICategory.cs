using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface ICategory
    {
        PaginatedList<Category> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        Category GetItem(Guid id);
        Category Greate(Category item);
        Category Edit(Category item);
        Category Delete(Category item);

        public bool IsItemNameExists(string name);
        public bool IsItemNameExists(string name, Guid id);
    }
}
