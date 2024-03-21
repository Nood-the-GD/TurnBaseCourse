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
                GridPosition mouseGriPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                GridPosition startGridPosition = new GridPosition(0, 0);

                List<GridPosition> path = PathFinding.Instance.FindPath(startGridPosition, mouseGriPosition);
                Debug.Log(path.Count);
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.Log(LevelGrid.Instance.GetWorldPosition(path[i]) != null);
                    Debug.Log(i.ToString());
                    Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(path[i]), LevelGrid.Instance.GetWorldPosition(path[i + 1]), Color.red, 10f);
                }
            }
        }
    }
}