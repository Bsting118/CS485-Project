/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship
{
    /// <summary>
    /// Class that mainly pilots the ship via applying forces to it, which are affected by a sourced ShipMovementInput.
    /// </summary>
    public class ShipController : MonoBehaviour
    {
        // Fields:
        [Header("Source Input To Control Aircraft")]
        [SerializeField] private ShipMovementInput _shipMovementInput; // this may be PlayerShipMovementInput (inherits)

        [Header("Aircraft Movement Values")]
        [SerializeField][Range(1000f, 10000f)] private float _thrustForce = 7500f;
        [SerializeField][Range(1000f, 10000f)] private float _yawForce = 2000f;
        [SerializeField][Range(1000f, 10000f)] private float _pitchForce = 6000f;
        [SerializeField][Range(1000f, 10000f)] private float _rollForce = 1000f;

        [Header("Aircraft Movement Affectors")]
        [SerializeField][Range(-1f, 1f)] private float _thrustFactor;
        [SerializeField][Range(-1f, 1f)] private float _yawFactor;
        [SerializeField][Range(-1f, 1f)] private float _pitchFactor;
        [SerializeField][Range(-1f, 1f)] private float _rollFactor;

        [Header("Ship Model Components")]
        [SerializeField] List<ShipEngine> _engines;

        private Rigidbody _thisRigidBody;
        IShipMovement SourcedInput => _shipMovementInput.ShipControls;

        #region MonoBehaviors
        void Awake()
        {
            _thisRigidBody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            foreach (ShipEngine engine in _engines)
            {
                engine.Init(SourcedInput);
            }
        }

        private void Update()
        {
            _thrustFactor = SourcedInput.ThrustFactorInput;
            _yawFactor = SourcedInput.YawFactorInput;
            _pitchFactor = SourcedInput.PitchFactorInput;
            _rollFactor = SourcedInput.RollFactorInput;
        }

        void FixedUpdate()
        {
            // Physics-update YAW:
            if (!Mathf.Approximately(0f, _yawFactor))
            {
                // Affect Y-axis constant:
                _thisRigidBody.AddTorque(transform.up * (_yawForce * _yawFactor * Time.fixedDeltaTime));
            }

            // Physics-update PITCH:
            if (!Mathf.Approximately(0f, _pitchFactor))
            {
                // Affect X-axis constant:
                _thisRigidBody.AddTorque(transform.right * (_pitchForce * _pitchFactor * Time.fixedDeltaTime));
            }

            // Physics-update ROLL:
            if (!Mathf.Approximately(0f, _rollFactor))
            {
                // Affect Z-axis constant:
                _thisRigidBody.AddTorque(transform.forward * (_rollForce * _rollFactor * Time.fixedDeltaTime));
            }

            // Physics-update THRUST:
            if (!Mathf.Approximately(0f, _thrustFactor))
            {
                _thisRigidBody.AddForce(transform.forward * (_thrustForce * _thrustFactor * Time.fixedDeltaTime));
            }
        }
        #endregion
    }
}