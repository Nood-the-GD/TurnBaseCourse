using NOOD;
using UnityEngine;

public class MouseWorld : MonoBehaviorInstance<MouseWorld>
{
    [SerializeField] private LayerMask _groundLayer;

    private void Update()
    {
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, MouseWorld.Instance._groundLayer);
        return hitInfo.point;
    }

    public static bool TryGetSelectedObjectWithLayer(LayerMask layerMask, out GameObject gameObject)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, layerMask))
        {
            gameObject = hitInfo.transform.gameObject;
            return true;
        }
        else
        {
            gameObject = null;
            return false;
        }
    }
}
