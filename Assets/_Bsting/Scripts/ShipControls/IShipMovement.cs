/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base interface to declare ship movement properties that are to be initialized by PlayerInput.
/// </summary>
public interface IShipMovement
{
    public float PitchFactorInput { get; }
    public float YawFactorInput { get; }
    public float ThrustFactorInput { get; }
    public float RollFactorInput { get; }
}
