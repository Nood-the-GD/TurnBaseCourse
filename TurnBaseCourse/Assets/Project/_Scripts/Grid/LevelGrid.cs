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
    void Start()
    {
        _gridSystem.CreateDebugObjects(_debugPref);
    }

    public GridPositionStruct GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    public void SetUnitAtGridPosition(GridPositionStruct gridPositionStruct, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPositionStruct);
        gridObject.SetUnit(unit);
    }
    public Unit GetUnitAtGridPosition(GridPositionStruct gridPositionStruct)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPositionStruct);
        return gridObject.GetUnit();
    }
    public void ClearUnitAtGridPosition(GridPositionStruct gridPositionStruct)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPositionStruct);
        gridObject.SetUnit(null);
    }

}
