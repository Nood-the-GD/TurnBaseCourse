using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening.Plugins.Options;
using NOOD;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace Game
{
    public class PathFinding : MonoBehaviorInstance<PathFinding>
    {
        #region Variables
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        [SerializeField] private Transform _gridDebugObjectPref;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private LayerMask _floorLayerMask;
        [SerializeField] private Transform _pathFindingLinks;

        private int _width;
        private int _height;
        private float _cellSize;
        private int _floorAmount;
        private List<GridSystem<PathNode>> _gridSystemPathNodeList;
        private List<PathFindingLink> _pathFindingLinkList;
        private Vector2Int[] _neighborVector = {
            new Vector2Int(0, 1), 
            new Vector2Int(1, 0), 
            new Vector2Int(0, -1), 
            new Vector2Int(-1, 0), 
            new Vector2Int(1, 1), 
            new Vector2Int(1, -1), 
            new Vector2Int(-1, 1), 
            new Vector2Int(-1, -1) };
        #endregion

        #region Unity functions
        protected override void ChildAwake()
        {
        }
        #endregion

        #region Setup
        public void Setup(int width, int height, float cellSize, int floorAmount)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            this._floorAmount = floorAmount;

            _gridSystemPathNodeList = new List<GridSystem<PathNode>>();
            _pathFindingLinkList = new List<PathFindingLink>();

            for(int floor = 0; floor < floorAmount; floor++)
            {
                GridSystem<PathNode> gridSystemPathNode = new GridSystem<PathNode>(_width, _height, _cellSize, floor, LevelGrid.FLOOR_HEIGHT, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
                _gridSystemPathNodeList.Add(gridSystemPathNode);
            }
            foreach(PathFindingLinkMono pathFindingLink in _pathFindingLinks.GetComponentsInChildren<PathFindingLinkMono>())
            {
                _pathFindingLinkList.Add(pathFindingLink.GetPathFindingLink());
            }
            CheckWalkableForAllGridPosition();
        }
        private void CheckWalkableForAllGridPosition()
        {
            for(int x = 0; x < _width; x++)
            {
                for(int z = 0; z < _height; z++)
                {
                    for(int floor = 0; floor < _floorAmount; floor ++)
                    {
                        GridPosition gridPosition = new GridPosition(x, z, floor);
                        Vector3 worldPos = LevelGrid.Instance.GetWorldPosition(gridPosition);
                        float raycastOffset = 1f;

                        GetNode(x, z, floor).SetIsWalkable(false);

                        if(Physics.Raycast(worldPos + Vector3.up * raycastOffset, Vector3.down, raycastOffset * 2, _floorLayerMask))
                        {
                            GetNode(x, z, floor).SetIsWalkable(true);
                        }

                        if(Physics.BoxCast(
                            worldPos + Vector3.down * raycastOffset, 
                            Vector3.one * 0.5f, 
                            Vector3.up, 
                            Quaternion.identity, 
                            raycastOffset * 2, 
                            _obstacleLayerMask))
                        {
                            GetNode(x, z, floor).SetIsWalkable(false);
                        }
                    }
                }
            }
        }
        #endregion

        #region Main functions
        public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
        {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();


            PathNode startNode = GetGridSystem(startGridPosition.floor).GetGridObject(startGridPosition);
            PathNode endNode = GetGridSystem(endGridPosition.floor).GetGridObject(endGridPosition);
            openList.Add(startNode);

            for(int x = 0; x < _width; x++)
            {
                for(int z = 0; z < _height; z++)
                {
                    for(int floor = 0; floor < _floorAmount; floor ++)
                    {
                        GridPosition gridPosition = new GridPosition(x, z, floor);
                        PathNode pathNode = GetGridSystem(floor).GetGridObject(gridPosition);

                        pathNode.SetGCost(int.MaxValue);
                        pathNode.SetHCost(0);
                        pathNode.CalculateFCost();
                        pathNode.ResetCameFromPathNode();
                    }
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
                    pathLength = endNode.GetFCost();
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighborNode in GetNeighborList(currentNode))
                {
                    if (closedList.Contains(neighborNode))
                    {
                        continue;
                    }

                    if(!neighborNode.GetIsWalkable())
                    {
                        closedList.Add(neighborNode);
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
            }

            pathLength = 0;
            return null;
        }
        #endregion

        #region Support
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
        public int CalculateDistance(GridPosition from, GridPosition to)
        {
            GridPosition gridPosition = from - to;
            int xDistance = Mathf.Abs(gridPosition.X);
            int zDistance = Mathf.Abs(gridPosition.Z);
            int remaining = Mathf.Abs(xDistance - zDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        }
        public bool IsWalkableGridPosition(GridPosition gridPosition)
        {
            return GetGridSystem(gridPosition.floor).GetGridObject(gridPosition).GetIsWalkable();
        }
        public bool HasPath(GridPosition startPos, GridPosition endPos)
        {
            return FindPath(startPos, endPos, out int pathLength) != null;
        }
        public int GetPathLength(GridPosition startPos, GridPosition endPos)
        {
            FindPath(startPos, endPos, out int length);
            return length;
        }
        public void UpdatePathFinding()
        {
            CheckWalkableForAllGridPosition();
        }
        #endregion

        #region Get
        private List<PathNode> GetNeighborList(PathNode currentNode)
        {
            List<PathNode> neighborList = new List<PathNode>();
            GridPosition gridPosition = currentNode.GetGridPosition();

            foreach(Vector2Int vectorNeighbor in _neighborVector)
            {
                GridPosition testGridPosition = new GridPosition(gridPosition.X + vectorNeighbor.x, gridPosition.Z + vectorNeighbor.y, gridPosition.floor);
                if (GetGridSystem(testGridPosition.floor).IsValidGridPosition(testGridPosition))
                {
                    neighborList.Add(GetGridSystem(testGridPosition.floor).GetGridObject(testGridPosition));
                }
            }

            List<PathNode> totalNeighborList = new List<PathNode>();
            totalNeighborList.AddRange(neighborList);

            List<GridPosition> pathFindingLinkGP = GetPathFindingLinkWithThisGridPosition(gridPosition);
            foreach(GridPosition grid in pathFindingLinkGP)
            {
                totalNeighborList.Add(GetNode(grid));
            }

            return totalNeighborList;
        }
        private List<GridPosition> GetPathFindingLinkWithThisGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> gridPositions = new List<GridPosition>();
            foreach(PathFindingLink pathFindingLink in _pathFindingLinkList)
            {
                if(pathFindingLink.GridPositionA == gridPosition)
                {
                    gridPositions.Add(pathFindingLink.GridPositionB);
                }
                if(pathFindingLink.GridPositionB == gridPosition)
                {
                    gridPositions.Add(pathFindingLink.GridPositionA);
                }
            }
            return gridPositions;
        }
        private GridSystem<PathNode> GetGridSystem(int floor)
        {
            return _gridSystemPathNodeList[floor];
        }
        private PathNode GetNode(int x, int z, int floor)
        {
            return GetGridSystem(floor).GetGridObject(new GridPosition(x, z, floor));
        }
        private PathNode GetNode(GridPosition gridPosition)
        {
            return GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        }
        #endregion
    }
}
