using System.Collections.Generic;

namespace Game
{
    public class GridObject
    {
        private GridSystem<GridObject> _gridSystem;
        private GridPosition _gridPositionStruct;
        private List<Unit> _unitList = new List<Unit>();

        public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPositionStruct)
        {
            this._gridSystem = gridSystem;
            this._gridPositionStruct = gridPositionStruct;
        }

        public override string ToString()
        {
            string unitString = "";
            foreach (Unit unit in _unitList)
            {
                unitString += unit;
            }
            return _gridPositionStruct.ToString() + unitString;
        }

        public void AddUnit(Unit unit)
        {
            this._unitList.Add(unit);
        }
        public void RemoveUnit(Unit unit)
        {
            this._unitList.Remove(unit);
        }
        public List<Unit> GetUnitList()
        {
            return _unitList;
        }

        public bool HasAnyUnit()
        {
            return _unitList.Count > 0;
        }
        public Unit GetUnit()
        {
            if (HasAnyUnit())
                return _unitList[0];
            else
                return null;
        }
    }
}