using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface IProductGroup
    {
        PaginatedList<ProductGroup> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        ProductGroup GetItem(Guid id);
        ProductGroup Greate(ProductGroup item);
        ProductGroup Edit(ProductGroup item);
        ProductGroup Delete(ProductGroup item);

        public bool IsItemNameExists(string name);
        public bool IsItemNameExists(string name, Guid id);
    }
}
