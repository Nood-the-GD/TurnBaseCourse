using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

namespace Game
{
    public class GridSystemVisual : MonoBehaviorInstance<GridSystemVisual>
    {
        #region SerializeField
        [SerializeField] private GridSystemVisualSingle _gridSystemSingleVisualPrefab;
        #endregion

        #region private
        private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;
        #endregion

        #region Unity Functions
        private void Start()
        {
            _gridSystemVisualSingleArray = new GridSystemVisualSingle[
                LevelGrid.Instance.GetWidth(), 
                LevelGrid.Instance.GetHeight()
            ];

            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    GridSystemVisualSingle gridSystemVisualSingle = Instantiate(_gridSystemSingleVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                    _gridSystemVisualSingleArray[x, z] = gridSystemVisualSingle;
                }
            }
            HideAllGridPosition();
        }
        private void Update()
        {
            UpdateGridVisual();
        }
        #endregion

        #region Update Visual 
        private void UpdateGridVisual()
        {
            HideAllGridPosition();
            BaseAction baseAction = UnitActionSystem.Instance.GetSelectedAction();
            ShowGridPositionList(baseAction.GetValidGridPositionList());
        }
        #endregion

        #region Show Hide
        public void HideAllGridPosition()
        {
            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    _gridSystemVisualSingleArray[x, z].Hide();
                }
            }
        }
        public void ShowGridPositionList(List<GridPosition> gridPositions)
        {
            foreach(var position in gridPositions)
            {
                GetGridSystemVisualSingle(position).Show(); 
            }
        }
        #endregion

        #region Get single
        public GridSystemVisualSingle GetGridSystemVisualSingle(GridPosition gridPosition)
        {
            return _gridSystemVisualSingleArray[gridPosition.X, gridPosition.Z];
        }
        #endregion
    }
}