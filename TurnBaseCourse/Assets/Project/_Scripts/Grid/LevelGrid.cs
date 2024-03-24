using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

namespace Game
{
    public class LevelGrid : MonoBehaviorInstance<LevelGrid>
    {
        public const float FLOOR_HEIGHT = 3f;

        #region Events
        public event EventHandler OnAnyUnitMoveGridPosition;
        #endregion

        #region Variables
        [SerializeField] private Transform _debugPref;
        [SerializeField] private int _width, _height;
        [SerializeField] private float _cellSize;
        [SerializeField] private int _floorAmount;
        private List<GridSystem<GridObject>> _gridSystemList;
        #endregion

        #region Unity functions
        protected override void ChildAwake()
        {
            _gridSystemList = new List<GridSystem<GridObject>>();
            for(int floor = 0; floor< _floorAmount; floor++)
            {
               GridSystem<GridObject> gridSystem = new GridSystem<GridObject>(_width, _height, _cellSize, floor, FLOOR_HEIGHT, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
                _gridSystemList.Add(gridSystem);
            }
        }
        void OnEnable()
        {
            PathFinding.Instance.Setup(_width, _height, _cellSize, _floorAmount);
        }
        #endregion

        #region Grid Zone
        /// <summary>
        /// Test if grid is null or not 
        /// </summary>
        /// <param name="gridPositionStruct"></param>
        /// <returns></returns>
        public bool IsValidGrid(GridPosition gridPositionStruct) 
        { 
            if(gridPositionStruct.floor < 0 || gridPositionStruct.floor >= _floorAmount)
            {
                return false;
            }
            else
            {
                return GetGridSystem(gridPositionStruct.floor).IsValidGridPosition(gridPositionStruct); 
            }
        }
        public GridPosition GetGridPosition(Vector3 worldPosition) 
        { 
            return GetGridSystem(GetFloor(worldPosition)).GetGridPosition(worldPosition); 
        }
        public Vector3 GetWorldPosition(GridPosition gridPositionStruct) => GetGridSystem(gridPositionStruct.floor).GetWorldPosition(gridPositionStruct);
        public int GetFloor(Vector3 worldPosition)
        {
            return Mathf.RoundToInt(worldPosition.y / FLOOR_HEIGHT);
        }
        #endregion

        #region Unit Zone
        public List<Unit> GetUnitListAtGridPosition(GridPosition gridPositionStruct)
        {
            GridObject gridObject = GetGridSystem(gridPositionStruct.floor).GetGridObject(gridPositionStruct);
            return gridObject.GetUnitList();
        }
        public void AddUnitAtGridPosition(GridPosition gridPositionStruct, Unit unit)
        {
            GridObject gridObject = GetGridSystem(gridPositionStruct.floor).GetGridObject(gridPositionStruct);
            gridObject.AddUnit(unit);
        }
        public void RemoveUnitAtGridPosition(GridPosition gridPositionStruct, Unit unit)
        {
            GridObject gridObject = GetGridSystem(gridPositionStruct.floor).GetGridObject(gridPositionStruct);
            gridObject.RemoveUnit(unit);
        }
        public void UnitMoveGridPosition(Unit unit, GridPosition from, GridPosition to)
        {
            RemoveUnitAtGridPosition(from, unit);
            AddUnitAtGridPosition(to, unit);
            OnAnyUnitMoveGridPosition?.Invoke(this, EventArgs.Empty);
        }
        public bool HasAnyUnitOnGridPosition(GridPosition gridPositionStruct)
        {
            return GetGridSystem(gridPositionStruct.floor).GetGridObject(gridPositionStruct).HasAnyUnit();
        }
        public Unit GetUnitAtGridPosition(GridPosition gridPositionStruct)
        {
            return GetGridSystem(gridPositionStruct.floor).GetGridObject(gridPositionStruct).GetUnit();
        }
        #endregion

        #region Interaction Zone
        public void AddInteractableObjectAtThisGrid(GridPosition gridPosition, IInteractable interactable)
        {
            GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
            gridObject.SetInteractableObject(interactable);
        }
        public void RemoveInteractableObjectAtThisGrid(GridPosition gridPosition)
        {
            GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
            gridObject.SetInteractableObject(null);
        }
        public IInteractable GetInteractableAtThisGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
            return gridObject.GetInteractableObject();
        }
        #endregion

        #region Get 
        public int GetWidth()
        {
            return GetGridSystem(0).GetWidth();
        }
        public int GetHeight()
        {
            return GetGridSystem(0).GetHeight();
        }
        public int GetFloorAmount()
        {
            return this._floorAmount;
        }
        public GridSystem<GridObject> GetGridSystem(int floor)
        {
            return _gridSystemList[floor];
        }
        #endregion
    }
}















