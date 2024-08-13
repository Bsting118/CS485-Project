/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Bsting.Ship.Player
{
    /// <summary>
    /// Class that inherits from ShipMovementBase, in which it reads from a hooked up PlayerInputActionsAsset "PlayerInputBinds", to determine ship movement values/factors. 
    /// </summary>
    public class PlayerShipMovement : ShipMovementBase
    {
        // Public fields:
        [SerializeField] public PlayerInputSystem PlayerInputBinds;
        [SerializeField] public float HyperspeedBoostAmount = 2.0f;
        [HideInInspector, SerializeField] public bool _isPitchInverted = false;
        [HideInInspector, SerializeField] public bool _isMouseFilteredToGameWindow = false; // Added
        [HideInInspector, SerializeField] public float _stepValueToEaseRoll = 1f;
        [HideInInspector, SerializeField] public float _stepValueToEaseThrust = 1f;

        // Private fields:
        [SerializeField] private float _deadzoneRadius = 0.1f;

        // Private var's:
        private Vector2 _foundScreenCenter => new Vector2(Screen.width * 0.5f, Screen.height * 0.5f); // lambda eval.
        private float _easedRollFactor = 0f;
        private float _easedThrustFactor = 0f;

        #region MAIN SPACESHIP INPUT VALUE ACCESSORS
        /* ========   MAIN SPACESHIP INPUT CONNECTORS/ACCESSORS ======== */
        public override float PitchFactorInput
        {
            get
            {
                // ...
                float currentMousePitchPos = PlayerInputBinds.Player.AircraftPitch.ReadValue<float>();
                float currentMouseYawPos = PlayerInputBinds.Player.AircraftYaw.ReadValue<float>(); // Added
                float finalPitchFactor;

                if (_isMouseFilteredToGameWindow)
                {
                    // Added if condition with else { return no pos; } so doesn't keep spinning ship after moving cursor outside active game window:
                    if ((!IsMouseYPositionOutOfGameWindow(currentMousePitchPos)) && (!IsMouseXPositionOutOfGameWindow(currentMouseYawPos)))
                    {
                        finalPitchFactor = FindDeltaPitch(currentMousePitchPos);
                    }
                    else
                    {
                        finalPitchFactor = 0f;
                    }
                }
                else
                {
                    finalPitchFactor = FindDeltaPitch(currentMousePitchPos);
                }

                return finalPitchFactor;
            }
        }

        public override float YawFactorInput
        {
            get
            {
                // ...
                float currentMouseYawPos = PlayerInputBinds.Player.AircraftYaw.ReadValue<float>();
                float currentMousePitchPos = PlayerInputBinds.Player.AircraftPitch.ReadValue<float>(); // Added
                float finalYawFactor;

                if (_isMouseFilteredToGameWindow)
                {
                    // Added if condition with else { return no pos; } so doesn't keep spinning ship after moving cursor outside active game window:
                    if ((!IsMouseXPositionOutOfGameWindow(currentMouseYawPos)) && (!IsMouseYPositionOutOfGameWindow(currentMousePitchPos)))
                    {
                        finalYawFactor = FindDeltaYaw(currentMouseYawPos);
                    }
                    else
                    {
                        finalYawFactor = 0f;
                    }
                }
                else
                {
                    finalYawFactor = FindDeltaYaw(currentMouseYawPos);
                }

                return finalYawFactor;
            }
        }

        public override float ThrustFactorInput
        {
            get
            {
                // ...
                float currentThrustSignedVal = PlayerInputBinds.Player.AircraftThrust.ReadValue<float>();

                // ADDED
                float currentEstimatedThrust = Mathf.Approximately(Mathf.Abs(currentThrustSignedVal), 0f) ? 0f : currentThrustSignedVal;
                _easedThrustFactor = Mathf.Lerp(_easedThrustFactor, currentEstimatedThrust, Time.deltaTime * _stepValueToEaseThrust);

                // If there's nothing sensed on the bind, then return no factor on thrust:
                return _easedThrustFactor;
            }
        }

        public override float RollFactorInput
        {
            get
            {
                // ...
                float currentRollSignedVal = PlayerInputBinds.Player.AircraftRoll.ReadValue<float>();

                // ADDED
                float currentEstimatedRoll = Mathf.Approximately(Mathf.Abs(currentRollSignedVal), 0f) ? 0f : currentRollSignedVal;
                _easedRollFactor = Mathf.Lerp(_easedRollFactor, currentEstimatedRoll, Time.deltaTime * _stepValueToEaseRoll);

                // If there's nothing sensed on the bind, then return no factor on thrust:
                return _easedRollFactor;
            }
        }

        public override float HyperspeedFactorInput
        {
            get
            {
                // ...
                float currentBoostSignedVal = PlayerInputBinds.Player.AircraftHyperSpeed.ReadValue<float>();

                // ADDING
                float currentEstimatedBoost = Mathf.Approximately(Mathf.Abs(currentBoostSignedVal), 0f) ? 1.0f : (currentBoostSignedVal * HyperspeedBoostAmount);

                return currentEstimatedBoost;
            }
        }
        /* ========   END OF MAIN SPACESHIP INPUT CONNECTORS/ACCESSORS ======== */
        #endregion

        #region Helper Function(s)
        private bool IsMouseXPositionOutOfGameWindow(float currentMouseXPos)
        {
            bool result = false;

            if ((currentMouseXPos < 0) || (currentMouseXPos > Screen.width))
            {
                result = true;
            }

            return result;
        }

        private bool IsMouseYPositionOutOfGameWindow(float currentMouseYPos)
        {
            bool result = false;

            if ((currentMouseYPos < 0) || (currentMouseYPos > Screen.height))
            {
                result = true;
            }

            return result;
        }

        private float FindDeltaPitch(float currentPitchPos)
        {
            float deltaPitchRatioFromCenter = (currentPitchPos - _foundScreenCenter.y) / _foundScreenCenter.y;
            if (!_isPitchInverted) { deltaPitchRatioFromCenter *= -1; }

            // If position (usually mouse) has been moved outside deadzone, then apply the new pitch:
            return Mathf.Abs(deltaPitchRatioFromCenter) > _deadzoneRadius ? deltaPitchRatioFromCenter : 0f;
        }

        private float FindDeltaYaw(float currentYawPos)
        {
            float deltaYawRatioFromCenter = (currentYawPos - _foundScreenCenter.x) / _foundScreenCenter.x;

            // If position (usually mouse) has been moved outside deadzone, then apply the new yaw:
            return Mathf.Abs(deltaYawRatioFromCenter) > _deadzoneRadius ? deltaYawRatioFromCenter : 0f;
        }
        #endregion
    }
}