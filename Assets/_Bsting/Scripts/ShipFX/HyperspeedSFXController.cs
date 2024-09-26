using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bsting.Ship.Managers;
using Bsting.Ship;

[RequireComponent(typeof(ShipController))]
public class HyperspeedSFXController : MonoBehaviour
{
    // Fields:
    [SerializeField] private ShipController _shipControllerOfHyperspeed = null;

    #region MonoBehaviors
    void OnEnable()
    {
        // Make sure we got the ship's controller on this same GameObject:
        if (_shipControllerOfHyperspeed == null)
        {
            _shipControllerOfHyperspeed = this.gameObject.GetComponent<ShipController>();
        }

        // Now listen-in on the Hyperspeed events to know when to play the SFX:
        _shipControllerOfHyperspeed.OnHyperspeedActivated.AddListener(PlayHyperspeedActiveEvent);
        _shipControllerOfHyperspeed.OnHyperspeedExpired.AddListener(PlayHyperspeedCoolingDownEvent);
        _shipControllerOfHyperspeed.OnHyperspeedReplenished.AddListener(PlayHyperspeedBackOnlineEvent);
    }

    void OnDisable()
    {
        _shipControllerOfHyperspeed.OnHyperspeedActivated.RemoveListener(PlayHyperspeedActiveEvent);
        _shipControllerOfHyperspeed.OnHyperspeedExpired.RemoveListener(PlayHyperspeedCoolingDownEvent);
        _shipControllerOfHyperspeed.OnHyperspeedReplenished.RemoveListener(PlayHyperspeedBackOnlineEvent);
    }
    #endregion

    #region Helper Method(s)
    private void PlayHyperspeedActiveEvent()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayHyperspeedActiveSFX();
        }
    }

    private void PlayHyperspeedCoolingDownEvent()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayHyperspeedOnCooldownSFX();
        }
    }

    private void PlayHyperspeedBackOnlineEvent()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayHyperspeedReadySFX();
        }
    }
    #endregion
}
