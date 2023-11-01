using Microsoft.Data.SqlClient;
using Stock.Models;

namespace Stock.Interfaces
{
    public interface IUnit
    {
        PaginatedList<Unit> GetUnits(string sortProperty, SortOrder order, string searchText, int pageIndex, int pageSize);
        Unit GetUnit(Guid id);
        Unit Greate(Unit unit);
        Unit Edit(Unit unit);
        Unit Delete(Unit unit);
    }
}
