using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DestructionCrate : MonoBehaviour
    {
        [SerializeField] private Transform _destroyCratePrefab;

        public void Damage(Vector3 damagePosition)
        {
            Transform crate = Instantiate(_destroyCratePrefab, this.transform.position, this.transform.rotation);
            ApplyExplosion(crate, 100f, damagePosition, 10f);
            Destroy(gameObject);
        }

        private void ApplyExplosion(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
        {
            foreach(Transform child in root)
            {
                if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
                {
                    childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);             
                }
                ApplyExplosion(child, explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}
