using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ScreenShakeAction : MonoBehaviour
    {
        #region Unity functions
        private void OnEnable()
        {
            ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
            GrenadeProjectile.onAnyGrenadeExploded += GrenadeProjectile_onAnyGrenadeExploded;
        }

        #endregion

        #region Events functions
        private void GrenadeProjectile_onAnyGrenadeExploded(object sender, EventArgs e)
        {
            ScreenShake.Instance.Shake(5);
        }
        private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
        {
            ScreenShake.Instance.Shake(1f);
        }
        #endregion
    }
}
