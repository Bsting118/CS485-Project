using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bsting.Ship.Managers;
using Unity.VisualScripting;

public class ReturnRemoteButton : MonoBehaviour
{
    // private fields:
    [SerializeField] private Button _attachedReturnToMenuButton = null;

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
        _attachedReturnToMenuButton.onClick.RemoveAllListeners();
    }

    private void TryToGetButtonOnThisObject()
    {
        if (_attachedReturnToMenuButton == null)
        {
            // Try to get the button using this script:
            _attachedReturnToMenuButton = this.gameObject.GetComponent<Button>();
        }
    }

    private void TryToAddUIReturnListener()
    {
        if (!_hasListenerBeenAdded)
        {
            if ((_attachedReturnToMenuButton != null) && (UIManager.Instance != null))
            {
                _attachedReturnToMenuButton.onClick.AddListener(UIManager.Instance.GoToMainMenu);
                _hasListenerBeenAdded = true;
            }
        }
    }
    
    private void TryToRemoveAllListeners()
    {
        if (_attachedReturnToMenuButton != null)
        {
            _attachedReturnToMenuButton.onClick.RemoveAllListeners();
        }
    }
}
