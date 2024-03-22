using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

namespace Game
{
    public class LevelGrid : MonoBehaviorInstance<LevelGrid>
    {
        public event EventHandler OnAnyUnitMoveGridPosition;
        [SerializeField] private Transform _debugPref;
        [SerializeField] private int _width, _height;
        [SerializeField] private float _cellSize;
        private GridSystem<GridObject> _gridSystem;

        protected override void ChildAwake()
        {
            _gridSystem = new GridSystem<GridObject>(_width, _height, _cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        }
        void OnEnable()
        {
            PathFinding.Instance.Setup(_width, _height, _cellSize);
        }

        #region Grid Zone
        /// <summary>
        /// Test if grid is null or not 
        /// </summary>
        /// <param name="gridPositionStruct"></param>
        /// <returns></returns>
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
        }
        public void RemoveUnitAtGridPosition(GridPosition gridPositionStruct, Unit unit)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPositionStruct);
            gridObject.RemoveUnit(unit);
        }
        public void UnitMoveGridPosition(Unit unit, GridPosition from, GridPosition to)
        {
            RemoveUnitAtGridPosition(from, unit);
            AddUnitAtGridPosition(to, unit);
            OnAnyUnitMoveGridPosition?.Invoke(this, EventArgs.Empty);
        }
        public bool HasAnyUnitOnGridPosition(GridPosition positionStruct)
        {
            return _gridSystem.GetGridObject(positionStruct).HasAnyUnit();
        }
        public Unit GetUnitAtGridPosition(GridPosition gridPositionStruct)
        {
            return _gridSystem.GetGridObject(gridPositionStruct).GetUnit();
        }
        #endregion

        #region Get 
        public int GetWidth()
        {
            return _width;
        }
        public int GetHeight()
        {
            return _height;
        }
        #endregion
    }
}