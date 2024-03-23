using UnityEngine;
using Cinemachine;
using NOOD;

namespace Game
{
    public class ScreenShake : MonoBehaviorInstance<ScreenShake>
    {
        private CinemachineImpulseSource _cinemachineImpulseSource;

        protected override void ChildAwake()
        {
            _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.T))
            {
            }    
        }

        public void Shake(float intensity = 1f)
        {
            _cinemachineImpulseSource.GenerateImpulse(intensity);
        }
    }
}
