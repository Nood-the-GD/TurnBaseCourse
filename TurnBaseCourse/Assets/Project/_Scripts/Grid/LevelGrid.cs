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
        private GridSystem _gridSystem;
        private int _width, _height;

        protected override void ChildAwake()
        {
            _width = 10;
            _height = 10;
            _gridSystem = new GridSystem(_width, _height, 2f);
        }
        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            _gridSystem.CreateDebugObjects(_debugPref);
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