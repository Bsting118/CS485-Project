using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship
{
    public class ShipEngine : MonoBehaviour
    {
        [SerializeField] public GameObject _thruster;
        IShipMovement _shipMovementControls;

        bool ThrustersEnabled => !Mathf.Approximately(0f, _shipMovementControls.ThrustFactorInput); // Enable thruster FX if engine is throttling

        // Update is called once per frame
        void Update()
        {
            ActivateThrusters();
        }

        void ActivateThrusters()
        {
            _thruster.GetComponent<TrailRenderer>().emitting = (ThrustersEnabled);
        }

        public void Init(IShipMovement movementControls)
        {
            _shipMovementControls = movementControls;
        }
    }
}