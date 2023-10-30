using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;

namespace Stock.Repositories
{
    public class UnitRepository : IUnit
    {
        private readonly InventoryContext _context;

        public UnitRepository(InventoryContext context)
        {
            _context = context;
        }

        public List<Unit> GetUnits(string sortProperty, SortOrder order)
        {
            var units = _context.Units.ToList();

            if(sortProperty.ToLower() == "name")
            {
                if (order == SortOrder.Ascending)
                    units = units.OrderBy(x => x.Name).ToList();
                else
                    units = units.OrderByDescending(x => x.Name).ToList();
            }
            else if(sortProperty.ToLower() == "description")
            {
                if (order == SortOrder.Ascending)
                    units = units.OrderBy(x => x.Description).ToList();
                else
                    units = units.OrderByDescending(x => x.Description).ToList();
            }

            return units;
        }

        public Unit GetUnit(Guid id) => _context.Units.FirstOrDefault(x => x.Id == id);


        public Unit Greate(Unit unit)
        {
            _context.Units.Add(unit);
            _context.SaveChanges();
            return unit;
        }

        public Unit Edit(Unit unit)
        {
            _context.Units.Attach(unit);
            _context.Entry(unit).State = EntityState.Modified;
            _context.SaveChanges();
            return unit;
        }

        public Unit Delete(Unit unit)
        {
            _context.Units.Attach(unit);
            _context.Entry(unit).State = EntityState.Deleted;
            _context.SaveChanges();
            return unit;
        }
    }
}
