using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface IProductGroup
    {
        PaginatedList<ProductGroup> GetUnits(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        ProductGroup GetUnit(Guid id);
        ProductGroup Greate(ProductGroup unit);
        ProductGroup Edit(ProductGroup unit);
        ProductGroup Delete(ProductGroup unit);

        public bool IsUnitNameExists(string name);
        public bool IsUnitNameExists(string name, Guid id);
    }
}
