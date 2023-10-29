using Stock.Models;

namespace Stock.Interfaces
{
    public interface IUnit
    {
        List<Unit> GetUnits();
        Unit GetUnit(Guid id);
        Unit Greate(Unit unit);
        Unit Edit(Unit unit);
        Unit Delete(Unit unit);
    }
}
