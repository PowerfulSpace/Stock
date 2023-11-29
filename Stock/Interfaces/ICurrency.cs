using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface ICurrency
    {
        PaginatedList<Currency> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        Currency GetItem(Guid id);
        Currency GetItem_NoDownload_FG(string code);
        bool Greate(Currency item);
        bool Edit(Currency item);
        bool Delete(Currency item);

        public bool IsItemNameExists(string name);
        public bool IsItemNameExists(string name, Guid id);
        public bool IsCurrencyCombExists(string srcIsCurrencyId, Guid excCurrencyId);
    }
}
