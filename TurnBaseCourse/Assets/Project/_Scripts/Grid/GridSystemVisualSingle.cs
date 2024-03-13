using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GridSystemVisualSingle : MonoBehaviour
    {
        #region SerializeField
        [SerializeField] private MeshRenderer _meshRender;
        #endregion

        #region Show Hide
        public void Show(Material material)
        {
            _meshRender.material = material;
            _meshRender.enabled = true;
        }
        public void Hide()
        {
            _meshRender.enabled = false;
        }
        #endregion
    }
}