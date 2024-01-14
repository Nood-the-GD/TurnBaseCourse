public class GridObject 
{
    private GridSystem _gridSystem;
    private GridPositionStruct _gridPositionStruct;
    private Unit _unit;

    public GridObject(GridSystem gridSystem, GridPositionStruct gridPositionStruct)
    {
        this._gridSystem = gridSystem;
        this._gridPositionStruct = gridPositionStruct;
    }

    public override string ToString()
    {
        return _gridPositionStruct.ToString() + _unit; 
    }

    public void SetUnit(Unit unit)
    {
        this._unit = unit;
    }
    public Unit GetUnit()
    {
        return _unit;
    }
}
