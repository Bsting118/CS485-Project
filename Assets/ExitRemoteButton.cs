using Bsting.Ship.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitRemoteButton : MonoBehaviour
{
    // private fields:
    [SerializeField] private Button _attachedExitButton = null;

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
    }

    void OnDestroy()
    {
        _attachedExitButton.onClick.RemoveAllListeners();
    }

    private void TryToGetButtonOnThisObject()
    {
        if (_attachedExitButton == null)
        {
            // Try to get the button using this script:
            _attachedExitButton = this.gameObject.GetComponent<Button>();
        }
    }

    private void TryToAddUIReturnListener()
    {
        if (!_hasListenerBeenAdded)
        {
            if ((_attachedExitButton != null) && (UIManager.Instance != null))
            {
                _attachedExitButton.onClick.AddListener(UIManager.Instance.ForceQuitGame);
                _hasListenerBeenAdded = true;
            }
        }
    }

    private void TryToRemoveAllListeners()
    {
        if (_attachedExitButton != null)
        {
            _attachedExitButton.onClick.RemoveAllListeners();
        }
    }
}
