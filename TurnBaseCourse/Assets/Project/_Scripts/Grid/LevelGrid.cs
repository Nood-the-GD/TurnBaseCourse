using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

public class LevelGrid : MonoBehaviorInstance<LevelGrid>
{
    [SerializeField] private Transform _debugPref;
    private GridSystem _gridSystem;


    protected override void ChildAwake()
    {
        _gridSystem = new GridSystem(4, 4, 2f);
    }
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        _gridSystem.CreateDebugObjects(_debugPref);
    }

    #region Grid Zone
    public bool IsValidGrid(GridPosition gridPositionStruct) => _gridSystem.IsValidGridPosition(gridPositionStruct);
    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPositionStruct) => _gridSystem.GetWorldPosition(gridPositionStruct);
    #endregion

    #region Unit Zone
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPositionStruct)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPositionStruct);
        return gridObject.GetUnitList();
    }
    public void AddUnitAtGridPosition(GridPosition gridPositionStruct, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPositionStruct);
        gridObject.AddUnit(unit);
        _gridSystem.UpdateGridDebugData();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPositionStruct, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPositionStruct);
        gridObject.RemoveUnit(unit);
        _gridSystem.UpdateGridDebugData();
    }
    public void UnitMoveGridPosition(Unit unit, GridPosition from, GridPosition to)
    {
        RemoveUnitAtGridPosition(from, unit);
        AddUnitAtGridPosition(to, unit);
    }
    public bool HasAnyUnitOnGridPosition(GridPosition positionStruct)
    {
        return _gridSystem.GetGridObject(positionStruct).HasAnyUnit();
    }
    #endregion

}
