using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PathFindingLinkMono : MonoBehaviour
    {
        public Vector3 LinkPositionA;
        public Vector3 LinkPositionB;

        public PathFindingLink GetPathFindingLink()
        {
            return new PathFindingLink
            {
                GridPositionA = LevelGrid.Instance.GetGridPosition(LinkPositionA),
                GridPositionB = LevelGrid.Instance.GetGridPosition(LinkPositionB)
            };
        }
    }
}
