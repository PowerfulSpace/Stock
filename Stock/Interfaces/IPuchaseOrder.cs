using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface IPuchaseOrder
    {
        PaginatedList<PoHeader> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        PoHeader GetItem(Guid id);
        bool Greate(PoHeader item);
        bool Edit(PoHeader item);
        bool Delete(PoHeader item);

        public bool IsPoNumberExists(string poNumber);
        public bool IsPoNumberExists(string poNumber, Guid id);

        public bool IsQuotationNoExists(string quoNumber);
        public bool IsQuotationNoExists(string quoNumber, Guid id);

        public string GetErrors();

    }
}
