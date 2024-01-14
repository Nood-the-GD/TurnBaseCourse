using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _debugText;
    private GridObject _gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this._gridObject = gridObject;
        UpdateGridData();
    }
    public void UpdateGridData()
    {
        _debugText.text = _gridObject.ToString();
    }
}
