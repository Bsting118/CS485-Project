/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

/// <summary>
/// Class that manages the overall game session, which includes mouse positions, PlayerInput instance(s), and quitting the game.
/// </summary>
public class GameManager : Manager<GameManager>
{
    [SerializeField] private bool _hideMouseCursor = true;
    [SerializeField] private bool _confineMouseCursorToGameWindow = true;
    [SerializeField] private bool _ignoreMouseCursorOutsideGameWindow = true;

    private PlayerInputSystem _currentPlayerInputSystem = null;

    #region MonoBehaviors
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        RestrictMouseInputToGameWindow(shouldHideCursor:_hideMouseCursor, shouldConfineCursor:_confineMouseCursorToGameWindow);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentPlayerInputSystem != null)
        {
            if (WasQuitOverrideTriggered())
            {
                QuitGame();
            }
        }
    }
    #endregion

    #region Helper Function(s)
    private void RestrictMouseInputToGameWindow(bool shouldHideCursor = false, bool shouldConfineCursor = false)
    {
        if (shouldConfineCursor)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (shouldHideCursor)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }

    public void SetPlayerInputInstance(PlayerInputSystem newInputInstance)
    {
        _currentPlayerInputSystem = newInputInstance;
    }

    public bool IsMouseIgnoredOutsideGameWindow()
    {
        bool result = _ignoreMouseCursorOutsideGameWindow;
        return result;
    }

    public bool WasQuitOverrideTriggered()
    {
        return _currentPlayerInputSystem.Player.QuitGameOverride.WasPressedThisFrame();
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // TODO: Handle WebGL shutdown case
        Application.Quit();
#endif
    }
    #endregion
}
