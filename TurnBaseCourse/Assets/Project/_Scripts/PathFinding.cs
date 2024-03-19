using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PathFinding : MonoBehaviour
    {
        [SerializeField] private Transform _gridDebugObjectPref;

        private int _width;
        private int _height;
        private int _cellSize;
        private GridSystem<PathNode> _gridSystemPathNode;

        private void Awake()
        {
            _gridSystemPathNode = new GridSystem<PathNode>(10, 10, 2f, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
            _gridSystemPathNode.CreateDebugObjects(_gridDebugObjectPref);
        }
    }
}
