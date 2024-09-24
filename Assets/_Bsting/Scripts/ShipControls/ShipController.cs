/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using Bsting.Ship.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bsting.Ship.Managers;

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
        [SerializeField] List<GameObject> _localShipCameras;

        [Header("Ship Abilities")]
        [SerializeField] private float _hyperSpeedDuration = 3.0f;
        [SerializeField] private float _hyperSpeedCooldownTime = 5.0f;
        public UnityEvent OnHyperspeedActivated = new UnityEvent(); // Use this to trigger hyperspeed VFX
        public UnityEvent OnHyperspeedExpired = new UnityEvent(); // Use this to turn off hyperspeed VFX

        private Rigidbody _thisRigidBody;
        private PlayerInputSystem _connectedInputMap = null;
        private PlayerShipMovementInput _initPlayerInputControls = null;
        private Coroutine _hyperspeedRoutine = null;
        private bool _canUseHyperspeed = true;
        private bool _isHyperspeedPressed = false;
        private float _amountOfHyperspeedRemaining;
        private float _timeLeftOnHyperspeedCooldown;
        private float _MAX_SPEED_FACTOR = 0f;
        private float _currentBoostSpeed = 0f;
        private float _currentHyperspeedMultiplier = 0f;
        private bool _hasHyperSpeedJumpstarted = false;
        private bool _hasLevelManagerBeenInit = false;

        IShipMovement SourcedInput => _shipMovementInput.ShipControls;

        #region MonoBehaviors
        void Awake()
        {
            _thisRigidBody = GetComponent<Rigidbody>();

            // ADDING:
            if (_shipMovementInput != null)
            {
                if (_shipMovementInput.GetType() == typeof(PlayerShipMovementInput))
                {
                    _initPlayerInputControls = _shipMovementInput as PlayerShipMovementInput;
                    _connectedInputMap = _initPlayerInputControls._playerInputMap;
                }
            }
        }

        void OnEnable()
        {
            // Setup or Fetch from Managers here (for Player scenes):
            SubscribeShipTransformToLevelManager();

            ReinitializeCameraManagerList();
        }

        void OnDisable()
        {
            if (_hyperspeedRoutine != null)
            {
                InterruptHyperspeedRoutine();
            }
        }

        void OnDestroy()
        {
            if (_hyperspeedRoutine != null)
            {
                InterruptHyperspeedRoutine();
            }
        }

        void Start()
        {
            if (!_hasLevelManagerBeenInit)
            {
                SubscribeShipTransformToLevelManager();
            }

            foreach (ShipEngine engine in _engines)
            {
                engine.Init(SourcedInput);
            }

            if (Mathf.Approximately(0f, _MAX_SPEED_FACTOR))
            {
                _MAX_SPEED_FACTOR = (_thrustForce * 1); // 1 is to represent a pressed Thrust input value 
            }
        }

        void Update()
        {
            // Make sure any dynamic local lists are setup:
            CheckToRefreshChildCameraList();

            // At the start of each frame, send updated transform info to Level Manager:
            LevelManager.Instance.SetPlayerShipTransform(GetTransformOfShip());

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
                if ((_hyperspeedFactor > 1.0f) && (_hyperspeedRoutine == null)) // Hyperspeed input is being pressed
                {
                    _currentHyperspeedMultiplier = _hyperspeedFactor;
                    _hyperspeedRoutine = StartCoroutine(UseHyperspeed()); // ADDING
                }
            }

            // Physics-update THRUST:
            if (Mathf.Approximately(0f, _thrustFactor) && _isHyperspeedPressed)
            {
                CheckIfHyperspeedHasBeenJumpstarted();
                _thisRigidBody.velocity = transform.forward * (_currentBoostSpeed * _currentHyperspeedMultiplier * Time.fixedDeltaTime);
                CheckIfBoostedSpeedExceedsMaxSpeed();
            }
            else if (!_connectedInputMap.Player.AircraftThrust.IsPressed() && _isHyperspeedPressed)
            {
                CheckIfHyperspeedHasBeenJumpstarted();
                _thisRigidBody.velocity = transform.forward * (_currentBoostSpeed * _currentHyperspeedMultiplier * Time.fixedDeltaTime);
                CheckIfBoostedSpeedExceedsMaxSpeed();
            }
            else if (!Mathf.Approximately(0f, _thrustFactor) && _isHyperspeedPressed)
            {
                CheckIfHyperspeedHasBeenJumpstarted();
                _currentBoostSpeed = _MAX_SPEED_FACTOR;
                _thisRigidBody.velocity = transform.forward * (_currentBoostSpeed * _currentHyperspeedMultiplier * Time.fixedDeltaTime);
                CheckIfBoostedSpeedExceedsMaxSpeed();
            }
            else if (!Mathf.Approximately(0f, _thrustFactor) && !_isHyperspeedPressed)
            {
                _thisRigidBody.AddForce(transform.forward * (_thrustForce * _thrustFactor * Time.fixedDeltaTime));
            }
            
        }
        #endregion

        #region Coroutine(s)
        IEnumerator UseHyperspeed()
        {
            ResetDurationAndCooldownBarValues();
            _canUseHyperspeed = false;
            _isHyperspeedPressed = true;
            if (_initPlayerInputControls != null) { _initPlayerInputControls.DisableAllBlasters(); } // Disable blasters while in boost mode
            OnHyperspeedActivated.Invoke();
            _currentBoostSpeed = (_thrustForce / 2);
            yield return new WaitForSeconds(_hyperSpeedDuration);

            _isHyperspeedPressed = false;
            if (_initPlayerInputControls != null) { _initPlayerInputControls.EnableAllBlasters(); }
            OnHyperspeedExpired.Invoke();
            Debug.Log("!!! Hyperspeed is out of fuel. Cooldown engaged. !!!");

            yield return new WaitForSeconds(_hyperSpeedCooldownTime); // Hand off control back to main loop while cooldown runs

            // Reset vars:
            _hyperspeedRoutine = null;
            ResetDurationAndCooldownBarValues();
            _currentBoostSpeed = 0f;
            _currentHyperspeedMultiplier = 0f;
            _hasHyperSpeedJumpstarted = false;
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

        // Backup Function in case things go south later in development:
        private void MakeDirectionOfRigidBodyGoForward(Rigidbody usingThisRigidbody)
        {
            Vector3 currentVelocity = usingThisRigidbody.velocity;
            var localVelocity = usingThisRigidbody.transform.InverseTransformDirection(currentVelocity);

            bool isRBMovingForward = (localVelocity.z > 0);

            if (!isRBMovingForward)
            {
                // Force/override velocity to go forward on RB:
                Vector3 normalizedLocalForwardVel = usingThisRigidbody.transform.InverseTransformDirection(transform.forward);
                usingThisRigidbody.velocity = usingThisRigidbody.transform.InverseTransformDirection(transform.forward);
            }
        }

        private void CheckIfBoostedSpeedExceedsMaxSpeed()
        {
            if ((_currentBoostSpeed > _MAX_SPEED_FACTOR) || (Mathf.Approximately(_MAX_SPEED_FACTOR, _currentBoostSpeed)))
            {
                _currentBoostSpeed = _MAX_SPEED_FACTOR;
            }
            else
            {
                _currentBoostSpeed = Mathf.Lerp(_currentBoostSpeed, _MAX_SPEED_FACTOR, Time.fixedDeltaTime);
            }
        }

        private void CheckIfHyperspeedHasBeenJumpstarted()
        {
            if (!_hasHyperSpeedJumpstarted)
            {
                _thisRigidBody.AddForce(transform.forward * (_currentBoostSpeed * _hyperspeedFactor * Time.fixedDeltaTime));
                _hasHyperSpeedJumpstarted = true;
            }
        }

        private void SubscribeShipTransformToLevelManager()
        {
            // Send initial Transform to Level Manager:
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.SetPlayerShipTransform(GetTransformOfShip());
                _hasLevelManagerBeenInit = true;
            }
        }

        private void CheckToRefreshChildCameraList()
        {
            if (_localShipCameras.Count <= 0)
            {
                // Refresh it:
                for (int index = 0; index < transform.childCount; index++)
                {
                    // Get the current child game object under the ship controller:
                    GameObject currentChild = transform.GetChild(index).gameObject;

                    // Check if its a ship camera, and if so, add to the local list of cameras:
                    if (currentChild.CompareTag("CockpitCamera") || currentChild.CompareTag("FollowCamera"))
                    {
                        _localShipCameras.Add(currentChild);
                    }
                }
            }
        }

        private void ReinitializeCameraManagerList()
        {
            if (CameraManager.Instance != null)
            {
                if (_localShipCameras != null)
                {
                    CameraManager.Instance.SetListOfManagedCameras(_localShipCameras);
                    Debug.Log("MSG: Reinitialized local ship cameras list on Camera Manager. Count of ship cameras: " + _localShipCameras.Count);
                }
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

        public Vector3 GetPositionOfShip()
        {
            return this.gameObject.transform.position;
        }

        public Transform GetTransformOfShip()
        {
            return this.gameObject.transform;
        }
        #endregion

        public void SetPositionOfShip(Vector3 newPos)
        {
            this.gameObject.transform.position = newPos;
        }

        public void InterruptHyperspeedRoutine()
        {
            StopCoroutine(_hyperspeedRoutine);
            _hyperspeedRoutine = null;
        }
    }
}