using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using UnityEngine.Rendering;

public class MouseWorld : MonoBehaviorInstance<MouseWorld>
{
    [SerializeField] private LayerMask _groundLayer;

    private void Update()
    {
        this.transform.position = GetPosition();
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, MouseWorld.Instance._groundLayer);
        return hitInfo.point;
    }
}
