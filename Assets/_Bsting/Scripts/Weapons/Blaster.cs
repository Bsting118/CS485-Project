using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bsting.Ship.Managers;

namespace Bsting.Ship.Weapons
{
    public class Blaster : MonoBehaviour
    {
        [SerializeField] TargetCrosshair _crosshairToAimBlasterAt;

        [SerializeField] BlasterProjectile _projectilePrefab;
        [SerializeField] Transform _muzzle;
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

            if (CanFire && _currentPlayerInputSystem.Player.Fire.WasPressedThisFrame()) 
            {
                FireProjectile();
                if (SFXManager.Instance != null)
                {
                    SFXManager.Instance.PlayBlasterFiredSFX();
                }
            }
        }

        void FireProjectile()
        {
            _cooldown = _cooldownTime;
            Instantiate(_projectilePrefab, _muzzle.position, transform.rotation);
        }

        public void SetPlayerInputInstance(PlayerInputSystem newInputInstance)
        {
            _currentPlayerInputSystem = newInputInstance;
        }
    }
}