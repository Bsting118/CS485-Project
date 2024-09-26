using Bsting.Ship.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameRemoteButton : MonoBehaviour
{
    // private fields:
    [SerializeField] private Button _attachedNewGameButton = null;

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
        _attachedNewGameButton.onClick.RemoveAllListeners();
        _hasListenerBeenAdded = false;
    }

    private void TryToGetButtonOnThisObject()
    {
        if (_attachedNewGameButton == null)
        {
            // Try to get the button using this script:
            _attachedNewGameButton = this.gameObject.GetComponent<Button>();
        }
    }

    private void TryToAddUIReturnListener()
    {
        if (!_hasListenerBeenAdded)
        {
            if ((_attachedNewGameButton != null) && (UIManager.Instance != null))
            {
                _attachedNewGameButton.onClick.AddListener(UIManager.Instance.PlayMainGame);
                _hasListenerBeenAdded = true;
            }
        }
    }

    private void TryToRemoveAllListeners()
    {
        if (_attachedNewGameButton != null)
        {
            _attachedNewGameButton.onClick.RemoveAllListeners();
        }
    }
}
