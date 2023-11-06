using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface IProductProfile
    {
        PaginatedList<ProductProfile> GetUnits(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        ProductProfile GetUnit(Guid id);
        ProductProfile Greate(ProductProfile unit);
        ProductProfile Edit(ProductProfile unit);
        ProductProfile Delete(ProductProfile unit);

        public bool IsUnitNameExists(string name);
        public bool IsUnitNameExists(string name, Guid id);
    }
}
