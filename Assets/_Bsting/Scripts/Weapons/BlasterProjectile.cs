using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bsting.Ship.Weapons
{
    public class BlasterProjectile : MonoBehaviour
    {
        [SerializeField]
        [Range(5000f, 25000f)]
        float _launchForce = 10000f;

        [SerializeField][Range(10, 1000)] int _damage = 100;
        [SerializeField][Range(2f, 10f)] float _range = 5f;
        [SerializeField] private float _projectileHitDistance = 10f;
        public LayerMask MasksForProjectileRay;

        private Rigidbody _rigidBody;
        private float _duration;
        private Ray _contactRay; // ADDED FOR BETTER COLLISION SYSTEM
        private RaycastHit _hitOutput;
        private Vector3 _originOfRay = Vector3.zero;

        bool OutOfFuel
        {
            get
            {
                _duration -= Time.deltaTime;
                return _duration <= 0f;
            }
        }

        void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            // Setup Ray to cast:
            _originOfRay = this.gameObject.transform.position;
            _contactRay = new Ray(_originOfRay, transform.forward); //transform.forward);

            // Apply launch to projectile:
            _rigidBody.AddForce(_launchForce * transform.forward); //transform.forward);

            // Set its time of flight (fuel/gunpowder):
            _duration = _range;
        }

        void Update()
        {
            if (OutOfFuel) { Destroy(gameObject); }
        }

        void FixedUpdate()
        {
            if (Physics.Raycast(_contactRay, out _hitOutput, _projectileHitDistance, MasksForProjectileRay))
            {
                ApplyDamageToHitObject(_hitOutput.collider, _hitOutput.point);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & MasksForProjectileRay) != 0)
            {
                ApplyDamageToHitObject(collision.collider, collision.GetContact(0).point);
            }
        }

        void OnDrawGizmos()
        {
            Ray simulatedRay;
            if (Application.isPlaying)
            {
                simulatedRay = _contactRay;
            }
            else
            {
                simulatedRay = new Ray(transform.position, transform.forward);
            }

            // Draw a yellow line representing the ray
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(simulatedRay.origin, simulatedRay.GetPoint(_projectileHitDistance));
        }

        private void ApplyDamageToHitObject(Collider colliderOfHitObject, Vector3 pointOfContact)
        {
            // Debug.Log("Hit object: " + colliderOfHitObject);

            IDamageable damageable = colliderOfHitObject.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Take damage at point-of-contact:
                damageable.TakeDamage(_damage, pointOfContact);
            }

            Destroy(gameObject);
        }
    }
}