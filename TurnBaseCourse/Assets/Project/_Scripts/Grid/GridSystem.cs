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
        private TGridObject[,] _gridObjectArray;

        public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createTGridObject)
        {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;

            _gridObjectArray = new TGridObject[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPositionStruct = new GridPosition(x, z);
                    _gridObjectArray[x, z] = createTGridObject(this, gridPositionStruct);
                    Debug.DrawLine(GetWorldPosition(gridPositionStruct), GetWorldPosition(gridPositionStruct) + Vector3.right * .1f, Color.white, 1000);
                }
            }
        }

        public Vector3 GetWorldPosition(GridPosition gridPositionStruct)
        {
            return new Vector3(gridPositionStruct.X, 0, gridPositionStruct.Z) * _cellSize;
        }

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition
            (
                Mathf.RoundToInt(worldPosition.x / _cellSize),
                Mathf.RoundToInt(worldPosition.z / _cellSize)
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
                    GridPosition gridPositionStruct = new GridPosition(x, z);

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