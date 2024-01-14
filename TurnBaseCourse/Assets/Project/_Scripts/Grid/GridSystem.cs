using UnityEngine;
using UnityEngine.Rendering;

public class GridSystem 
{
    private int _width;
    private int _height;
    private float _cellSize;
    private GridObject[,] _gridObjectArray;

    public GridSystem(int width, int height, float cellSize)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;

        _gridObjectArray = new GridObject[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPositionStruct gridPositionStruct = new GridPositionStruct(x, z);
                _gridObjectArray[x, z] = new GridObject(this, gridPositionStruct);
                Debug.DrawLine(GetWorldPosition(gridPositionStruct), GetWorldPosition(gridPositionStruct) + Vector3.right * .1f, Color.white, 1000);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPositionStruct gridPositionStruct)
    {
        return new Vector3(gridPositionStruct.X, 0, gridPositionStruct.Z) * _cellSize;
    }

    public GridPositionStruct GetGridPosition(Vector3 worldPosition)
    {
        return new GridPositionStruct
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
                GridPositionStruct gridPositionStruct = new GridPositionStruct(x, z);
                 
                Transform debugTransform = GameObject.Instantiate(debugPref, GetWorldPosition(gridPositionStruct), debugPref.rotation);

                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();

                gridDebugObject.SetGridObject(GetGridObject(gridPositionStruct));
            }
        }
    }

    public GridObject GetGridObject(GridPositionStruct gridPositionStruct)
    {
        return _gridObjectArray[gridPositionStruct.X, gridPositionStruct.Z];
    }
}
