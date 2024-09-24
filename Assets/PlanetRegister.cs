using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bsting.Ship.Managers;

public class PlanetRegister : MonoBehaviour
{
    private bool _hasPlanetBeenRegistered = false;

    #region MonoBehaviors
    void Update()
    {
        if (!_hasPlanetBeenRegistered)
        {
            TryToCheckShadedLevelRegistryForIncludedPlanet();
        }
       
    }
    #endregion

    #region Helper Function(s)
    private void CheckIfPlanetIsInsideLevelRegistry()
    {
        foreach (GameObject shadedObj in LevelManager.Instance.TargetShadedObjectsToFacePlayerDirection)
        {
            if (GameObject.ReferenceEquals(shadedObj, this.gameObject))
            {
                _hasPlanetBeenRegistered = true;
                break;
            }
        }
    }

    private void TryToCheckShadedLevelRegistryForIncludedPlanet()
    {
        if (LevelManager.Instance != null)
        {
            CheckIfPlanetIsInsideLevelRegistry();

            if (!_hasPlanetBeenRegistered)
            {
                LevelManager.Instance.TargetShadedObjectsToFacePlayerDirection.Add(this.gameObject);
                Debug.Log("MSG: Refreshed one of Level Manager's shaded list of planets.");
                _hasPlanetBeenRegistered = true;
            }
        }
    }
    #endregion
}
