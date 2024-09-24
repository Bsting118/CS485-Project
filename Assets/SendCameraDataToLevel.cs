using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bsting.Ship.Managers;

public class SendCameraDataToLevel : MonoBehaviour
{
    private bool _hasFirstPersonCamBeenSent = false;

    // Update is called once per frame
    void Update()
    {
        TryToSendFirstPersonCamDataToLevel();
    }

    private void TryToSendFirstPersonCamDataToLevel()
    {
        if (!_hasFirstPersonCamBeenSent)
        {
            if (LevelManager.Instance != null)
            {
                if (LevelManager.Instance.FirstPerson3DCamera == null)
                {
                    LevelManager.Instance.SetFirstPerson3DCamera(this.gameObject);
                    _hasFirstPersonCamBeenSent = true;
                }
                else
                {
                    _hasFirstPersonCamBeenSent = true;
                }
            }
        }
    }
}
