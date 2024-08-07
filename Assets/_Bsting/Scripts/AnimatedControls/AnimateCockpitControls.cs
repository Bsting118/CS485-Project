/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Bsting.Ship.Player.Animation
{
    /// <summary>
    /// Class to animate the cockpit prefab's objects to the movement of its ship. 
    /// </summary>
    public class AnimateCockpitControls : MonoBehaviour
    {
        [Header("Flight Control Transforms And Ranges")]
        [SerializeField] private Transform _joystick;
        [SerializeField] private Vector3 _joystickRange = Vector3.zero;
        [SerializeField] List<Transform> _throttles = new List<Transform>();
        [SerializeField] private float _throttleRange = 35.0f;

        [Header("Flight Inputs To Sync Animation To")]
        [SerializeField] private ShipMovementInput _movementInputToSyncTo;
        IShipMovement movementControlInput => _movementInputToSyncTo.ShipControls;

        #region MonoBehaviors
        // Update is called once per frame
        void Update()
        {
            // Update cockpit controls movement to match ship input:
            SyncJoystickRotation();

            SyncThrottleRotation();
        }
        #endregion

        private void SyncJoystickRotation()
        {
            // Update joystick rotation:
            _joystick.localRotation = Quaternion.Euler(
                movementControlInput.PitchFactorInput * _joystickRange.x,
                movementControlInput.YawFactorInput * _joystickRange.y,
                movementControlInput.RollFactorInput * _joystickRange.z);  // Convert from Euler angles to Quarternion (transform uses a Vector3)
        }

        private void SyncThrottleRotation()
        {
            // Update throttle rotation:
            Vector3 throttleRotation = _throttles[0].localRotation.eulerAngles;
            throttleRotation.x = movementControlInput.ThrustFactorInput * _throttleRange;

            // --> Apply to each throttle handle in list:
            foreach (Transform throttle in _throttles)
            {
                throttle.localRotation = Quaternion.Euler(throttleRotation);
            }
        }
    }
}