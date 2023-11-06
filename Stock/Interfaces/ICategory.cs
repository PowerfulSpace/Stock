using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface ICategory
    {
        PaginatedList<Category> GetUnits(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        Category GetUnit(Guid id);
        Category Greate(Category unit);
        Category Edit(Category unit);
        Category Delete(Category unit);

        public bool IsUnitNameExists(string name);
        public bool IsUnitNameExists(string name, Guid id);
    }
}
