using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Testing : MonoBehaviour
    {
        List<GridPosition> gridPositions = new List<GridPosition>() { new GridPosition(1, 1), new GridPosition(2, 2) };

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.T))
            {
            }
        }
    }
}