using System.Collections;
using System.Collections.Generic;
using Bsting.Ship.Managers;
using UnityEngine;
using UnityEngine.UI;

public class SettingsRemoteButton : MonoBehaviour
{
    // private fields:
    [SerializeField] private Button _attachedSettingsButton = null;

    // private var's:
    private bool _hasListenerBeenAdded = false;

    void OnEnable()
    {
        TryToGetButtonOnThisObject();

        TryToAddUIReturnListener();
    }

    void Start()
    {
        TryToGetButtonOnThisObject();

        TryToAddUIReturnListener();
    }

    void OnDisable()
    {
        TryToRemoveAllListeners();
        _hasListenerBeenAdded = false;
    }

    void OnDestroy()
    {
        _attachedSettingsButton.onClick.RemoveAllListeners();
        _hasListenerBeenAdded = false;
    }

    private void TryToGetButtonOnThisObject()
    {
        if (_attachedSettingsButton == null)
        {
            // Try to get the button using this script:
            _attachedSettingsButton = this.gameObject.GetComponent<Button>();
        }
    }

    private void TryToAddUIReturnListener()
    {
        if (!_hasListenerBeenAdded)
        {
            if ((_attachedSettingsButton != null) && (UIManager.Instance != null))
            {
                _attachedSettingsButton.onClick.AddListener(UIManager.Instance.GoToSettingsMenu);
                _hasListenerBeenAdded = true;
            }
        }
    }

    private void TryToRemoveAllListeners()
    {
        if (_attachedSettingsButton != null)
        {
            _attachedSettingsButton.onClick.RemoveAllListeners();
        }
    }
}
