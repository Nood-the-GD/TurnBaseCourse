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

        public PathNode(GridPosition gridPosition)
        {
            _gridPosition = gridPosition;
        }

        public override string ToString()
        {
            return _gridPosition.ToString();
        }

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
        public void SetGCost(int gCost)
        {
            _gCost = gCost;
        }
        public void SetHCost(int hCost)
        {
            _hCost = hCost;
        }
        public void SetFCost(int fCost)
        {
            _fCost = fCost;
        }
    }
}
