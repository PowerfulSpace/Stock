using Microsoft.Data.SqlClient;
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
        public PaginatedList<Unit> GetUnits(string sortProperty, SortOrder order, string searchText,int pageIndex,int pageSize)
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

            PaginatedList<Unit> retUnits = new PaginatedList<Unit>(units, pageIndex, pageSize);

            return retUnits;
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

        public bool IsUnitNameExists(string name)
        {
            int ct = _context.Units.Where(x => x.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsUnitNameExists(string name,Guid id)
        {
            int ct = _context.Units.Where(x => x.Name.ToLower() == name.ToLower() && x.Id != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
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


    }
}
