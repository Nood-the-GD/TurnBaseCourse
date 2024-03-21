using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

namespace Game
{
    public class PathFinding : MonoBehaviorInstance<PathFinding>
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        [SerializeField] private Transform _gridDebugObjectPref;

        private int _width;
        private int _height;
        private int _cellSize;
        private GridSystem<PathNode> _gridSystemPathNode;
        private Vector2Int[] _neighborVector = {
            new Vector2Int(0, 1), 
            new Vector2Int(1, 0), 
            new Vector2Int(0, -1), 
            new Vector2Int(-1, 0), 
            new Vector2Int(1, 1), 
            new Vector2Int(1, -1), 
            new Vector2Int(-1, 1), 
            new Vector2Int(-1, -1) };

        protected override void ChildAwake()
        {
            _gridSystemPathNode = new GridSystem<PathNode>(10, 10, 2f, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
            _gridSystemPathNode.CreateDebugObjects(_gridDebugObjectPref);
        }

        public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();


            PathNode startNode = _gridSystemPathNode.GetGridObject(startGridPosition);
            PathNode endNode = _gridSystemPathNode.GetGridObject(endGridPosition);
            openList.Add(startNode);

            for(int x = 0; x < _gridSystemPathNode.GetWidth(); x++)
            {
                for(int z = 0; z < _gridSystemPathNode.GetHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    PathNode pathNode = _gridSystemPathNode.GetGridObject(gridPosition);

                    pathNode.SetGCost(int.MaxValue);
                    pathNode.SetHCost(0);
                    pathNode.CalculateFCost();
                    pathNode.ResetCameFromPathNode();
                }
            }

            startNode.SetGCost(0);
            startNode.SetHCost(CalculateDistance(startGridPosition, startGridPosition));
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = openList[0];

                if(currentNode == endNode)
                {
                    // Reach final node
                    return CalculatePath(endNode);
                }

                foreach (PathNode neighborNode in GetNeighborList(currentNode))
                {
                    if (closedList.Contains(neighborNode))
                    {
                        continue;
                    }

                    int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighborNode.GetGridPosition());
                    if (tentativeGCost < neighborNode.GetGCost())
                    {
                        neighborNode.SetCameFromPathNode(currentNode);
                        neighborNode.SetGCost(tentativeGCost);
                        neighborNode.SetHCost(CalculateDistance(neighborNode.GetGridPosition(), endGridPosition));
                        neighborNode.CalculateFCost();

                        if(!openList.Contains(neighborNode))
                        {
                            openList.Add(neighborNode);
                        }
                    }   
                }
                openList.Remove(currentNode);
                closedList.Add(currentNode);
            }
            return null;
        }

        private List<GridPosition> CalculatePath(PathNode endNode)
        {
            List<GridPosition> finalPath = new List<GridPosition>();

            finalPath.Add(endNode.GetGridPosition());
            PathNode currentNode = endNode;
            while (currentNode.GetCameFromPathNode() != null)
            {
                finalPath.Add(currentNode.GetCameFromPathNode().GetGridPosition());
                currentNode = currentNode.GetCameFromPathNode();
            }
            finalPath.Reverse();
            return finalPath;
        }

        private PathNode GetNode(int x, int z)
        {
            return _gridSystemPathNode.GetGridObject(new GridPosition(x, z));
        }

        private List<PathNode> GetNeighborList(PathNode currentNode)
        {
            List<PathNode> neighborList = new List<PathNode>();
            GridPosition gridPosition = currentNode.GetGridPosition();

            
            foreach(Vector2Int vectorNeighbor in _neighborVector)
            {
                GridPosition testGridPosition = new GridPosition(gridPosition.X + vectorNeighbor.x, gridPosition.Z + vectorNeighbor.y);
                if (_gridSystemPathNode.IsValidGridPosition(testGridPosition))
                {
                    neighborList.Add(_gridSystemPathNode.GetGridObject(testGridPosition));
                }
            }

            return neighborList;
        }

        public int CalculateDistance(GridPosition from, GridPosition to)
        {
            GridPosition gridPosition = from - to;
            int xDistance = Mathf.Abs(gridPosition.X);
            int zDistance = Mathf.Abs(gridPosition.Z);
            int remaining = Mathf.Abs(xDistance - zDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostPathNode = pathNodeList[0];
            for(int i = 0; i < pathNodeList.Count; i++)
            {
                if(pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
                {
                    lowestFCostPathNode = pathNodeList[i];
                }
            }
            return lowestFCostPathNode;
        }
    }
}
