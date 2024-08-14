/*-------------------------------------
 Custom script edited by Brendan Sting
 Original Author: Evan Brisita
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class that ensures the attached GameObject is only instanced once. 
/// </summary>
/// <typeparam name="T">Class Type To Become A Singleton</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static Singleton<T> Instance { get; protected set; }
    [SerializeField] protected bool _isDestroyedOnLoad = false;

    #region MonoBehaviors
    protected virtual void Awake()
    {
        LoadInstance();
    }
    #endregion

    #region Helper Function(s)
    protected void LoadInstance()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            Instance = this as T;

            if (!_isDestroyedOnLoad)
            {
                // Allow for a parent object to become the singleton holder:
                if (transform.parent != null)
                {
                    DontDestroyOnLoad(transform.parent.gameObject);
                }
                else
                {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
        }
    }
    #endregion
}
