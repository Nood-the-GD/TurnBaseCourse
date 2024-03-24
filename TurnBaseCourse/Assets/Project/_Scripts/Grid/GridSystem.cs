using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public class GridSystem<TGridObject>
    {
        private int _width;
        private int _height;
        private float _cellSize;
        private int _floor;
        private float _floorHeight;
        private TGridObject[,] _gridObjectArray;

        public GridSystem(int width, int height, float cellSize, int floor, float floorHeight, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createTGridObject)
        {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._floor = floor;
            this._floorHeight = floorHeight;

            _gridObjectArray = new TGridObject[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPositionStruct = new GridPosition(x, z, floor);
                    _gridObjectArray[x, z] = createTGridObject(this, gridPositionStruct);
                }
            }
        }

        public Vector3 GetWorldPosition(GridPosition gridPositionStruct)
        {
            Vector3 floorOffset = new Vector3(0, _floor, 0) * _floorHeight;
            return new Vector3(gridPositionStruct.X, 0, gridPositionStruct.Z) * _cellSize + floorOffset;
        }

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition
            (
                Mathf.RoundToInt(worldPosition.x / _cellSize),
                Mathf.RoundToInt(worldPosition.z / _cellSize),
                _floor
            );
        }

        public int GetWidth()
        {
            return _width;
        }
        public int GetHeight()
        {
            return _height;
        }

        public void CreateDebugObjects(Transform debugPref)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    GridPosition gridPositionStruct = new GridPosition(x, z, _floor);

                    Transform debugTransform = GameObject.Instantiate(debugPref, GetWorldPosition(gridPositionStruct), debugPref.rotation);

                    GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();

                    TGridObject gridObject = GetGridObject(gridPositionStruct);
                                        
                    gridDebugObject.SetGridObject(gridObject);

                }
            }
        }

        public TGridObject GetGridObject(GridPosition gridPositionStruct)
        {
            try
            {
                return _gridObjectArray[gridPositionStruct.X, gridPositionStruct.Z];
            }
            catch
            {
                return default;
            }
        }

        public bool IsValidGridPosition(GridPosition gridPositionStruct)
        {
            return GetGridObject(gridPositionStruct) != null;
        }
    }
}