using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface IProduct
    {
        PaginatedList<Product> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        Product GetItem(string code);
        Product GetItem_NoDownload_FG(string code);
        Product Greate(Product item);
        Product Edit(Product item);
        Product Delete(Product item);

        public bool IsItemNameExists(string name);
        public bool IsItemNameExists(string name, string code);

        public bool IsItemCodeNameExists(string itemCode);
        public bool IsItemCodeNameExists(string name, string itemCode);
    }
}
