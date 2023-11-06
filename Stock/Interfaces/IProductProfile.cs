using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface IProductProfile
    {
        PaginatedList<ProductProfile> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        ProductProfile GetItem(Guid id);
        ProductProfile Greate(ProductProfile item);
        ProductProfile Edit(ProductProfile item);
        ProductProfile Delete(ProductProfile item);

        public bool IsItemNameExists(string name);
        public bool IsItemNameExists(string name, Guid id);
    }
}
