/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using Bsting.Ship.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private float _hyperspeedFactor;

        [Header("Ship Model Components")]
        [SerializeField] List<ShipEngine> _engines;

        [Header("Ship Abilities")]
        [SerializeField] private float _hyperSpeedDuration = 3.0f;
        [SerializeField] private float _hyperSpeedCooldownTime = 5.0f;
        public UnityEvent OnHyperspeedActivated = new UnityEvent(); // Use this to trigger hyperspeed VFX
        public UnityEvent OnHyperspeedExpired = new UnityEvent(); // Use this to turn off hyperspeed VFX

        private Rigidbody _thisRigidBody;
        private PlayerInputSystem _connectedInputMap = null;
        private Coroutine _hyperspeedRoutine = null;
        private bool _canUseHyperspeed = true;
        private bool _isHyperspeedPressed = false;
        private float _amountOfHyperspeedRemaining;
        private float _timeLeftOnHyperspeedCooldown;
        private float _MAX_SPEED_FACTOR = 0f;
        private float _currentBoostSpeed = 0f;

        IShipMovement SourcedInput => _shipMovementInput.ShipControls;

        #region MonoBehaviors
        void Awake()
        {
            _thisRigidBody = GetComponent<Rigidbody>();
            // ResetDurationAndCooldownBarValues();

            // ADDING:
            if (_shipMovementInput != null)
            {
                if (_shipMovementInput.GetType() == typeof(PlayerShipMovementInput))
                {
                    PlayerShipMovementInput temp = _shipMovementInput as PlayerShipMovementInput;
                    _connectedInputMap = temp._playerInputMap;
                }
            }
        }

        void Start()
        {
            foreach (ShipEngine engine in _engines)
            {
                engine.Init(SourcedInput);
            }

            if (Mathf.Approximately(0f, _MAX_SPEED_FACTOR))
            {
                _MAX_SPEED_FACTOR = (_thrustForce * 1);
            }
        }

        void Update()
        {
            // Get processed input values from interface to Input Actions:
            _thrustFactor = SourcedInput.ThrustFactorInput;
            _yawFactor = SourcedInput.YawFactorInput;
            _pitchFactor = SourcedInput.PitchFactorInput;
            _rollFactor = SourcedInput.RollFactorInput;
            _hyperspeedFactor = SourcedInput.HyperspeedFactorInput;

            if (_isHyperspeedPressed)
            {
                Debug.Log("Time left in Hyperspeed: " + _amountOfHyperspeedRemaining);
                _amountOfHyperspeedRemaining -= Time.deltaTime;
            }
            else if (!_canUseHyperspeed && !_isHyperspeedPressed)
            {
                Debug.Log("Cooldown time remaining on Hyperspeed: " + _timeLeftOnHyperspeedCooldown);
                _timeLeftOnHyperspeedCooldown -= Time.deltaTime;
            }
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

            // "If we can use hyperspeed ability, and it has not been pressed yet":
            if (_canUseHyperspeed && !_isHyperspeedPressed) 
            {
                if (_hyperspeedFactor > 1.0f) // Hyperspeed input is being pressed
                {
                    _hyperspeedRoutine = StartCoroutine(UseHyperspeed()); // ADDING
                }
            }

            // Physics-update THRUST:
            // TODO: Change out .AddForce() with setting the RigidBody's velocity directly instead and then ease to reset back to 0 once hyperspeed is done
            if (Mathf.Approximately(0f, _thrustFactor) && _isHyperspeedPressed)
            {
                _thisRigidBody.AddForce(transform.forward * (_currentBoostSpeed * _hyperspeedFactor * Time.fixedDeltaTime));
                CheckIfBoostedSpeedExceedsMaxSpeed();
            }
            else if (!_connectedInputMap.Player.AircraftThrust.IsPressed() && _isHyperspeedPressed)
            {
                _thisRigidBody.AddForce(transform.forward * (_currentBoostSpeed * _hyperspeedFactor * Time.fixedDeltaTime));
                CheckIfBoostedSpeedExceedsMaxSpeed();
            }
            else if (!Mathf.Approximately(0f, _thrustFactor) && _isHyperspeedPressed)
            {
                _currentBoostSpeed += (_thrustForce * _thrustFactor);
                CheckIfBoostedSpeedExceedsMaxSpeed();
                _thisRigidBody.AddForce(transform.forward * (_currentBoostSpeed * _hyperspeedFactor * Time.fixedDeltaTime));
                CheckIfBoostedSpeedExceedsMaxSpeed();
            }
            else if (!Mathf.Approximately(0f, _thrustFactor) && !_isHyperspeedPressed)
            {
                _thisRigidBody.AddForce(transform.forward * (_thrustForce * _thrustFactor * Time.fixedDeltaTime));
            }
            Debug.Log("New current boost speed = " + _currentBoostSpeed);
        }
        #endregion

        #region Coroutine(s)
        IEnumerator UseHyperspeed()
        {
            ResetDurationAndCooldownBarValues();

            _canUseHyperspeed = false;
            _isHyperspeedPressed = true;
            OnHyperspeedActivated.Invoke();
            _currentBoostSpeed = (_thrustForce / 2);
            yield return new WaitForSeconds(_hyperSpeedDuration);

            _isHyperspeedPressed = false;
            OnHyperspeedExpired.Invoke();
            Debug.Log("!!! Hyperspeed is out of fuel. Cooldown engaged. !!!");

            yield return new WaitForSeconds(_hyperSpeedCooldownTime); // Hand off control back to main loop while cooldown runs

            // Reset vars:
            _hyperspeedRoutine = null;
            ResetDurationAndCooldownBarValues();
            _currentBoostSpeed = 0f;
            _canUseHyperspeed = true;
            Debug.Log("=== Cooldown expired. Hyperspeed is ready! ===");
        }
        #endregion

        #region Helper Function(s)
        private void ResetDurationAndCooldownBarValues()
        {
            _amountOfHyperspeedRemaining = _hyperSpeedDuration;
            _timeLeftOnHyperspeedCooldown = _hyperSpeedCooldownTime;
        }

        private void CheckIfBoostedSpeedExceedsMaxSpeed()
        {
            if ((_currentBoostSpeed > _MAX_SPEED_FACTOR) || (Mathf.Approximately(_MAX_SPEED_FACTOR, _currentBoostSpeed)))
            {
                _currentBoostSpeed = _MAX_SPEED_FACTOR;
            }
            else
            {
                _currentBoostSpeed += Mathf.Lerp(_currentBoostSpeed, _MAX_SPEED_FACTOR, Time.fixedDeltaTime);
            }
        }
        #endregion

        #region Public Accessor(s)
        public float GetRemainingHyperspeedAmount()
        {
            float currentAmtRemaining = _amountOfHyperspeedRemaining;
            return currentAmtRemaining;
        }

        public float GetTimeLeftOnHyperspeedCooldown()
        {
            float currentTimeLeft = _timeLeftOnHyperspeedCooldown;
            return currentTimeLeft;
        }
        #endregion
    }
}