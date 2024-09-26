using Bsting.Ship.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoBackRemoteButton : MonoBehaviour
{
    // private fields:
    [SerializeField] private Button _attachedGoBackButton = null;

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
        _attachedGoBackButton.onClick.RemoveAllListeners();
        _hasListenerBeenAdded = false;
    }

    private void TryToGetButtonOnThisObject()
    {
        if (_attachedGoBackButton == null)
        {
            // Try to get the button using this script:
            _attachedGoBackButton = this.gameObject.GetComponent<Button>();
        }
    }

    private void TryToAddUIReturnListener()
    {
        if (!_hasListenerBeenAdded)
        {
            if ((_attachedGoBackButton != null) && (UIManager.Instance != null))
            {
                _attachedGoBackButton.onClick.AddListener(UIManager.Instance.GoBack);
                _hasListenerBeenAdded = true;
            }
        }
    }

    private void TryToRemoveAllListeners()
    {
        if (_attachedGoBackButton != null)
        {
            _attachedGoBackButton.onClick.RemoveAllListeners();
        }
    }
}
