using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PathNode
    {
        public GridPosition _gridPosition;
        private int _gCost;
        private int _hCost;
        private int _fCost;
        private PathNode _parent;
        private bool _isWalkable = true;

        public PathNode(GridPosition gridPosition)
        {
            _gridPosition = gridPosition;
        }

        #region Get
        public int GetGCost()
        {
            return _gCost;
        }
        public int GetHCost()
        {
            return _hCost;
        }
        public int GetFCost()
        {
            return _fCost;
        }
        public GridPosition GetGridPosition()
        {
            return _gridPosition;
        }
        public PathNode GetCameFromPathNode()
        {
            return _parent;
        }
        public bool GetIsWalkable()
        {
            return _isWalkable;
        }
        #endregion

        #region Set
        public void SetGCost(int gCost)
        {
            _gCost = gCost;
        }
        public void SetHCost(int hCost)
        {
            _hCost = hCost;
        }
        public void SetCameFromPathNode(PathNode pathNode)
        {
            _parent = pathNode;
        }
        public void SetIsWalkable(bool isWalkable)
        {
            this._isWalkable = isWalkable; 
        }
        #endregion

        #region Support
        public void CalculateFCost()
        {
            _fCost = _gCost + _hCost;
        }
        public void ResetCameFromPathNode()
        {
            _parent = null;
        }
        public override string ToString()
        {
            return _gridPosition.ToString();
        }
        #endregion
    }
}
