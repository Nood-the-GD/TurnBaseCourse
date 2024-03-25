using System;
using System.Collections;
using System.Collections.Generic;
using log4net.Core;
using NOOD;
using NOOD.Extension;
using PlasticPipe.PlasticProtocol.Client;
using UnityEngine;

namespace Game
{
    public class GridSystemVisual : MonoBehaviorInstance<GridSystemVisual>
    {
        [Serializable]
        public struct GridVisualTypeMaterial
        {
            public GridVisualType gidVisualType;
            public Material material;
        }
        public enum GridVisualType
        {
            White,
            Blue,
            Red,
            RedSoft,
            Yellow
        }

        #region SerializeField
        [SerializeField] private GridSystemVisualSingle _gridSystemSingleVisualPrefab;
        [SerializeField] private List<GridVisualTypeMaterial> _gridVisualTypeMaterials;
        #endregion

        #region private
        private GridSystemVisualSingle[,,] _gridSystemVisualSingleArray;
        #endregion

        #region Unity Functions
        private void OnEnable()
        {
            UnitActionSystem.Instance.OnSelectActionChange += UnitActionSystem_OnSelectActionChangeHandler;
            UnitActionSystem.Instance.OnSelectUnitChange += UnitActionSystem_OnSelectUnitChangeHandler;
            UnitActionSystem.Instance.OnActionBusyChange += UnitActionSystem_OnActionBusyChange;
        }


        private void Start()
        {
            _gridSystemVisualSingleArray = new GridSystemVisualSingle[
                LevelGrid.Instance.GetWidth(), 
                LevelGrid.Instance.GetHeight(),
                LevelGrid.Instance.GetFloorAmount()
            ];

            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    for (int floor = 0; floor < LevelGrid.Instance.GetFloorAmount(); floor++)
                    {
                        GridPosition gridPosition = new GridPosition(x, z, floor);
                        GridSystemVisualSingle gridSystemVisualSingle = Instantiate(_gridSystemSingleVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                        _gridSystemVisualSingleArray[x, z, floor] = gridSystemVisualSingle;
                    }
                }
            }

            UpdateGridVisual();
        }
        private void OnDisable()
        {
            NoodyCustomCode.UnSubscribeAllEvent<UnitActionSystem>(this);
        }
        #endregion

        #region Event functions
        private void UnitActionSystem_OnActionBusyChange(object sender, bool e)
        {
            UpdateGridVisual();
        }
        private void UnitActionSystem_OnSelectActionChangeHandler(object sender, EventArgs eventArgs)
        {
            UpdateGridVisual();
        }
        private void UnitActionSystem_OnSelectUnitChangeHandler(object sender, EventArgs eventArgs)
        {
            UpdateGridVisual();
        }
        #endregion

        #region Update Visual 
        private void UpdateGridVisual()
        {
            HideAllGridPosition();
            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            BaseAction baseAction = UnitActionSystem.Instance.GetSelectedAction();

            GridVisualType gridVisualType = GridVisualType.White;
            switch (baseAction)
            {
                case MoveAction moveAction:
                    gridVisualType = GridVisualType.White;
                    break;
                case InteractionAction interactionAction:
                case SpinAction spinAction:
                    gridVisualType = GridVisualType.Blue;
                    break;
                case ShootAction shootAction:
                    gridVisualType = GridVisualType.Red;
                    ShowGridPositionRange(selectedUnit.GetCurrentGridPosition(), shootAction.GetRange(), GridVisualType.RedSoft);
                    break;
                case SwordAction swordAction:
                    gridVisualType = GridVisualType.Red;
                    ShowGridPositionRange(selectedUnit.GetCurrentGridPosition(), swordAction.GetRange(), GridVisualType.RedSoft, true);
                    break;
                case GrenadeAction grenadeAction:
                    gridVisualType = GridVisualType.Yellow;
                    break;
                default:
                    break;
            }
            ShowGridPositionList(baseAction.GetValidGridPositionList(), gridVisualType);
        }
        #endregion

        #region Show Hide
        public void HideAllGridPosition()
        {
            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    for(int floor = 0; floor < LevelGrid.Instance.GetFloorAmount(); floor++)
                    {
                        GridSystemVisualSingle gridSystemVisualSingle = GetGridSystemVisualSingle(new GridPosition(x, z, floor));
                        if (gridSystemVisualSingle == null) continue;
                        gridSystemVisualSingle.Hide();
                    }
                }
            }
        }
        public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
        {
            foreach(var position in gridPositions)
            {
                GetGridSystemVisualSingle(position)?.Show(GetGridVisualTypeMaterial(gridVisualType)); 
            }
        }
        private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType, bool isSquare = false)
        {
            List<GridPosition> gridPositionList = new List<GridPosition>();

            for (int x = -range; x <= range; x++)
            {
                for(int z = -range; z <= range; z++)
                {
                    for(int floor = -range; floor <= range; floor++)
                    {
                        GridPosition testGridPos = gridPosition + new GridPosition(x, z, floor);

                        if(!LevelGrid.Instance.IsValidGrid(testGridPos))
                        {
                            continue;
                        }

                        if(isSquare) 
                        {
                            gridPositionList.Add(testGridPos);
                            continue;
                        }
                        int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                        if(testDistance > range)
                        {
                            continue;
                        }

                        gridPositionList.Add(testGridPos);       
                    }
                }
            }
            ShowGridPositionList(gridPositionList, gridVisualType);
        }
        #endregion

        #region Get 
        public GridSystemVisualSingle GetGridSystemVisualSingle(GridPosition gridPosition)
        {
            if (_gridSystemVisualSingleArray == null) return null;
            return _gridSystemVisualSingleArray[gridPosition.X, gridPosition.Z, gridPosition.floor];
        }
        private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
        {
            foreach(GridVisualTypeMaterial gridVisualTypeMaterial in _gridVisualTypeMaterials)
            {
                if(gridVisualTypeMaterial.gidVisualType == gridVisualType)
                {
                    return gridVisualTypeMaterial.material;
                }
            }

            Debug.Log("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
            return null;
        }
        #endregion
    }
}