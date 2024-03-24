using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using Cinemachine;

namespace Game
{
    public class ScreenShake : MonoBehaviorInstance<ScreenShake>
    {
        private CinemachineImpulseSource _cinemachineImpulseSource;

        protected override void ChildAwake()
        {
            _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void Shake(float insensitive = 1f)
        {
            _cinemachineImpulseSource.GenerateImpulse(insensitive);
        }
    }
}
