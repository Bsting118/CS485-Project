/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class to hook up a ShipControls (part of IShipMovement interface) property with a provided ship input type.
/// </summary>
public class ShipMovementInput : MonoBehaviour
{
    // Properties:
    public IShipMovement ShipControls { get; protected set; }

    // Fields:
    // (Default is Player type)
    [SerializeField] protected ShipInputHandler.InputType _inputTypeForThisShip = ShipInputHandler.InputType.Player;

    #region MonoBehaviors
    protected virtual void Awake()
    {
        // ...
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ShipControls = ShipInputHandler.GetInputControls(_inputTypeForThisShip);
    }

    protected virtual void OnDestroy()
    {
        ShipControls = null;
    }
    #endregion
}
