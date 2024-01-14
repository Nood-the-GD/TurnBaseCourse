using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GridSystem 
{
    private int _width;
    private int _height;
    private float _cellSize;
    private GridObject[,] _gridObjectArray;
    private Dictionary<GridObject, GridDebugObject> _gridDebugObjectDic = new Dictionary<GridObject, GridDebugObject>();

    public GridSystem(int width, int height, float cellSize)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;

        _gridObjectArray = new GridObject[width,height];
        
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPositionStruct = new GridPosition(x, z);
                _gridObjectArray[x, z] = new GridObject(this, gridPositionStruct);
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
            Mathf.RoundToInt(worldPosition.x/_cellSize),
            Mathf.RoundToInt(worldPosition.z/_cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPref)
    {
        for(int x = 0; x < _width; x++)
        {
            for(int z = 0; z < _height; z++)
            {
                GridPosition gridPositionStruct = new GridPosition(x, z);
                 
                Transform debugTransform = GameObject.Instantiate(debugPref, GetWorldPosition(gridPositionStruct), debugPref.rotation);

                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();

                GridObject gridObject = GetGridObject(gridPositionStruct);
                gridDebugObject.SetGridObject(gridObject);
                _gridDebugObjectDic.TryAdd(gridObject, gridDebugObject);
            }
        }
    }

    public void UpdateGridDebugData()
    {
        for(int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GridObject gridObject = GetGridObject(gridPosition);
                if(_gridDebugObjectDic.TryGetValue(gridObject, out GridDebugObject debugObject))
                {
                    debugObject.UpdateGridData();
                }
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPositionStruct)
    {
        try
        {
            return _gridObjectArray[gridPositionStruct.X, gridPositionStruct.Z];
        }
        catch
        {
            return null;
        }
    }

    public bool IsValidGridPosition(GridPosition gridPositionStruct)
    {
        return GetGridObject(gridPositionStruct) != null;
    }
}
