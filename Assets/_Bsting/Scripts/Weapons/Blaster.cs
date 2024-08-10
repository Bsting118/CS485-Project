using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship.Weapons
{
    public class Blaster : MonoBehaviour
    {
        [SerializeField] TargetCrosshair _crosshairToAimBlasterAt;

        [SerializeField] BlasterProjectile _projectilePrefab;
        [SerializeField] Transform _muzzle;
        //[SerializeField] float _projectileSpawnForwardOffset = 0.0f;
        [SerializeField]
        [Range(0f, 5f)] float _cooldownTime = 0.25f;
        

        bool CanFire
        {
            get
            {
                _cooldown -= Time.deltaTime;
                return _cooldown <= 0f;
            }
        }

        private float _cooldown;
        private PlayerInputSystem _currentPlayerInputSystem;

        // Update is called once per frame
        void Update()
        {
            if (_crosshairToAimBlasterAt != null)
            {
                this.gameObject.transform.LookAt(_crosshairToAimBlasterAt.GetCrosshairPointToAimAt());
            }

            if (CanFire && _currentPlayerInputSystem.Player.Fire.WasPressedThisFrame()) // TODO: Change to new input system
            {
                FireProjectile();
            }
        }

        void FireProjectile()
        {
            _cooldown = _cooldownTime;
            Instantiate(_projectilePrefab, _muzzle.position, transform.rotation);
            //Vector3 spawnPos = new Vector3(_muzzle.position.x, _muzzle.position.y, _muzzle.position.z + _projectileSpawnForwardOffset);
            //Instantiate(_projectilePrefab, spawnPos, _projectilePrefab.transform.rotation);
        }

        public void SetPlayerInputInstance(PlayerInputSystem newInputInstance)
        {
            _currentPlayerInputSystem = newInputInstance;
        }
    }
}