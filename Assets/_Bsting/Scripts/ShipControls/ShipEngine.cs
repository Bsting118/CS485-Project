using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bsting.Ship.Managers;

namespace Bsting.Ship
{
    public class ShipEngine : MonoBehaviour
    {
        [SerializeField] public GameObject _thruster;
        IShipMovement _shipMovementControls;

        bool ThrustersEnabled => !Mathf.Approximately(0f, _shipMovementControls.ThrustFactorInput); // Enable thruster FX if engine is throttling
        private bool _overrideThrustersOff = false;
        private bool _hasSubscribedToTPEvent = false;

        void OnEnable()
        {
            SubscribeToPlayerTeleport();
        }

        void OnDisable()
        {
            UnsubscribeToPlayerTeleport();
        }

        void OnDestroy()
        {
            UnsubscribeToPlayerTeleport();
        }

        private void Start()
        {
            if (!_hasSubscribedToTPEvent)
            {
                SubscribeToPlayerTeleport();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!_overrideThrustersOff)
            {
                ActivateThrusters();
            }
        }

        private void ActivateThrusters()
        {
            TrailRenderer attachedThruster = _thruster.GetComponent<TrailRenderer>();
            if (attachedThruster != null)
            {
                attachedThruster.emitting = (ThrustersEnabled);
            }
        }

        private void TurnThrustersOff()
        {
            // Override and set emitting to being off:
            TrailRenderer attachedThruster = _thruster.GetComponent<TrailRenderer>();
            if (attachedThruster != null)
            {
                _overrideThrustersOff = true;
                attachedThruster.emitting = false;
                attachedThruster.enabled = false;
            }
        }

        private void TurnThrustersOn()
        {
            // Disable override and let throttle expression take back control:
            TrailRenderer attachedThruster = _thruster.GetComponent<TrailRenderer>();
            if (attachedThruster != null)
            {
                _overrideThrustersOff = false;
                attachedThruster.enabled = true;
            }
        }

        private void SubscribeToPlayerTeleport()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnTeleportPlayerReady.AddListener(TurnThrustersOff);
                LevelManager.Instance.OnPlayerTeleported.AddListener(TurnThrustersOn);
                _hasSubscribedToTPEvent = true;
            }
        }

        private void UnsubscribeToPlayerTeleport()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnTeleportPlayerReady.RemoveListener(TurnThrustersOff);
                LevelManager.Instance.OnPlayerTeleported.RemoveListener(TurnThrustersOn);
                _hasSubscribedToTPEvent = false;
            }
        }

        public void Init(IShipMovement movementControls)
        {
            _shipMovementControls = movementControls;
        }
    }
}