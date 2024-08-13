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
    /// Base class that's used to enforce inheritance from the core ship movement interface; this class provides an abstraction layer.
    /// </summary>
    public abstract class ShipMovementBase : IShipMovement
    {
        public abstract float PitchFactorInput { get; }
        public abstract float YawFactorInput { get; }
        public abstract float ThrustFactorInput { get; }
        public abstract float RollFactorInput { get; }
        public abstract float HyperspeedFactorInput { get; }
    }
}