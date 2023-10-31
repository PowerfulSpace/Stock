using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Models;
using System.Linq;

namespace Stock.Repositories
{
    public class UnitRepository : IUnit
    {
        private readonly InventoryContext _context;

        public UnitRepository(InventoryContext context)
        {
            _context = context;
        }

        private List<Unit> DoSort(List<Unit> units, string sortProperty, SortOrder order)
        {
            if (sortProperty.ToLower() == "name")
            {
                if (order == SortOrder.Ascending)
                    units = units.OrderBy(x => x.Name).ToList();
                else
                    units = units.OrderByDescending(x => x.Name).ToList();
            }
            else if (sortProperty.ToLower() == "description")
            {
                if (order == SortOrder.Ascending)
                    units = units.OrderBy(x => x.Description).ToList();
                else
                    units = units.OrderByDescending(x => x.Description).ToList();
            }

            return units;
        }

        public List<Unit> GetUnits(string sortProperty, SortOrder order, string searchText)
        {
            List<Unit> units;

            if(searchText != "" && searchText != null)
            {
                units = _context.Units
                    .Where(x => x.Name.Contains(searchText) || x.Description.Contains(searchText))
                    .ToList();
            }
            else
            {
                units = _context.Units.ToList();
            }

            units = DoSort(units, sortProperty, order);

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
