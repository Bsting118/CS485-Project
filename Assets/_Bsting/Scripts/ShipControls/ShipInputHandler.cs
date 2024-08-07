/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bsting.Ship.Player;

namespace Bsting.Ship
{
    /// <summary>
    /// Class to handle configuring ShipMovement input values and how their InputActionMaps are hooked up. 
    /// </summary>
    public class ShipInputHandler : MonoBehaviour
    {
        #region Enumeration Over Input Types
        public enum InputType
        {
            Player,
            EnemyAI
        }
        #endregion

        #region Helper Function(s)
        public static IShipMovement GetInputControls(InputType inputType,
                                                     PlayerInputSystem playerInputMap = null,
                                                     bool shouldInvertPitch = false,
                                                     bool shouldFilterMousePos = false,
                                                     float stepFactorToEaseRoll = 1.0f,
                                                     float stepFactorToEaseThrust = 1.0f)
        {
            IShipMovement determinedSource = null;

            switch (inputType)
            {
                case InputType.Player:
                    // Instantiate:
                    PlayerShipMovement newPlayerMovement = new PlayerShipMovement();

                    // Setup:
                    if (playerInputMap != null) { newPlayerMovement.PlayerInputBinds = playerInputMap; }

                    // Override input configs:
                    newPlayerMovement._isPitchInverted = shouldInvertPitch;
                    newPlayerMovement._isMouseFilteredToGameWindow = shouldFilterMousePos;
                    newPlayerMovement._stepValueToEaseRoll = stepFactorToEaseRoll;
                    newPlayerMovement._stepValueToEaseThrust = stepFactorToEaseThrust;

                    // Copy over interface:
                    determinedSource = newPlayerMovement;
                    break;
                case InputType.EnemyAI:
                    // Null for now; placeholder...
                    determinedSource = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null);
            }

            return determinedSource;
        }
        #endregion
    }
}