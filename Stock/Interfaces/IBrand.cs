using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface IBrand
    {
        PaginatedList<Brand> GetUnits(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        Brand GetUnit(Guid id);
        Brand Greate(Brand unit);
        Brand Edit(Brand unit);
        Brand Delete(Brand unit);

        public bool IsUnitNameExists(string name);
        public bool IsUnitNameExists(string name, Guid id);
    }
}
