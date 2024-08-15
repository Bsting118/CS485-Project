/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bsting.Ship.Managers;
using Bsting.Ship.Weapons;

namespace Bsting.Ship.Player
{
    /// <summary>
    /// Class that sets up the PlayerInputSystem with a GameManager and initializes the ship controls to be used in ShipController.
    /// </summary>
    public class PlayerShipMovementInput : ShipMovementInput
    {
        public PlayerInputSystem _playerInputMap { get; private set; }

        [SerializeField] private List<Blaster> _listOfConnectedBlasters;
        [SerializeField] private bool _realisticJoyStickEnabled = true;
        [SerializeField] private bool _filterMouseMovement = false;
        [SerializeField] private float _factorToEaseShipRollBy = 1.0f;
        [SerializeField] private float _factorToEaseShipThrustBy = 1.0f;
        [SerializeField] private float _factorToBoostShipHyperspeedBy = 3.0f;

        #region MonoBehaviors
        protected override void Awake()
        {
            base.Awake();

            // Player-specific config:
            _playerInputMap = new PlayerInputSystem();
            _playerInputMap.Enable();

            ConnectAllShipBlasters();

        }

        void OnEnable()
        {
            // Send to Game Manager:
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetPlayerInputInstance(_playerInputMap);
                _filterMouseMovement = GameManager.Instance.IsMouseIgnoredOutsideGameWindow();
            }

            if (CameraManager.Instance != null)
            {
                CameraManager.Instance.SetPlayerInputInstance(_playerInputMap);
            }
        }

        protected override void Start()
        {
            // Apply player input action map to player config:
            ShipControls = ShipInputHandler.GetInputControls(_inputTypeForThisShip,
                                                             playerInputMap: _playerInputMap,
                                                             shouldInvertPitch: _realisticJoyStickEnabled,
                                                             shouldFilterMousePos: _filterMouseMovement,
                                                             stepFactorToEaseRoll: _factorToEaseShipRollBy,
                                                             stepFactorToEaseThrust: _factorToEaseShipThrustBy,
                                                             hyperspeedBoostFactor: _factorToBoostShipHyperspeedBy);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        #endregion

        private void ConnectAllShipBlasters()
        {
            foreach (Blaster connectedBlaster in _listOfConnectedBlasters)
            {
                if (connectedBlaster != null)
                {
                    connectedBlaster.SetPlayerInputInstance(_playerInputMap);
                }
            }
        }

        public void DisableAllBlasters()
        {
            foreach (Blaster connectedBlaster in _listOfConnectedBlasters)
            {
                connectedBlaster.enabled = false;
            }
        }

        public void EnableAllBlasters()
        {
            foreach (Blaster connectedBlaster in _listOfConnectedBlasters)
            {
                connectedBlaster.enabled = true;
            }
        }
    }
}