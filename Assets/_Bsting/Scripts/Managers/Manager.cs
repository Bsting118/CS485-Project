/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class that inherits from a generic Singleton to ensure basic managers are only instanced once.
/// </summary>
public class Manager<T> : Singleton<T> where T: Manager<T>
{
    // ...
    #region MonoBehaviors
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion
}
