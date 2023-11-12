using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface ISupplier
    {
        PaginatedList<Supplier> GetItems(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        Supplier GetItem(Guid id);
        Supplier Greate(Supplier item);
        Supplier Edit(Supplier item);
        Supplier Delete(Supplier item);

        public bool IsItemNameExists(string name);
        public bool IsItemNameExists(string name, Guid id);

        public bool IsItemCodeExists(string code);
        public bool IsItemCodeExists(string code, Guid id);

        public bool IsItemEmailExists(string email);
        public bool IsItemEmailExists(string email, Guid id);
    }
}
