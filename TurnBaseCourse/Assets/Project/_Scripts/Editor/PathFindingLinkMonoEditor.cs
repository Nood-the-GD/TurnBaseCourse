using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(PathFindingLinkMono))]
    public class PathFindingLinkMonoEditor : Editor
    {
            
        public void OnScreenGUI()
        {
            PathFindingLinkMono pathFindingLinkMono = (PathFindingLinkMono)target;
            EditorGUI.BeginChangeCheck();
            Vector3 newLinkPositionA = Handles.PositionHandle(pathFindingLinkMono.LinkPositionA, Quaternion.identity);
            if(EditorGUI.EndChangeCheck())
            {
                pathFindingLinkMono.LinkPositionA = newLinkPositionA;
            }
        }
    }
}
