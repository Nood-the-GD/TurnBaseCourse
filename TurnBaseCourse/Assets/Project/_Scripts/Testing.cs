using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Testing : MonoBehaviour
    {
        List<GridPosition> gridPositions = new List<GridPosition>() { new GridPosition(1, 1), new GridPosition(2, 2) };

        // IEnumerator Start()
        // {
        //     yield return new WaitForSeconds(1);
        //     GridSystemVisual.Instance.ShowGridPositionList(gridPositions);
        // }
    }
}