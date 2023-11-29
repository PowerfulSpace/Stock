using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface ICurrency
    {
        PaginatedList<Currency> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        Currency GetItem(string code);
        Currency GetItem_NoDownload_FG(string code);
        Currency Greate(Currency item);
        Currency Edit(Currency item);
        Currency Delete(Currency item);

        public bool IsItemNameExists(string name);
        public bool IsItemNameExists(string name, string code);

        public bool IsItemCodeNameExists(string itemCode);
        public bool IsItemCodeNameExists(string name, string itemCode);
    }
}
