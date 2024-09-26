/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bsting.Ship.Managers
{
    /// <summary>
    /// Class that manages the overall game session, which includes mouse positions, PlayerInput instance(s), and quitting the game.
    /// </summary>
    public class GameManager : Manager<GameManager>
    {
        // Properties:
        // (Will have to wipe and refresh this on scene changes)
        [field: SerializeField] public PauseUIController PauseController = null; 

        // Fields:
        [SerializeField] private bool _hideMouseCursor = true;
        [SerializeField] private bool _confineMouseCursorToGameWindow = true;
        [SerializeField] private bool _ignoreMouseCursorOutsideGameWindow = true;
        [SerializeField] private bool _enableQuitGameOverrideOnTab = false;

        // Private var's:
        private PlayerInputSystem _currentPlayerInputSystem = null;
        private bool _gameHasBeenPaused = false;
        private bool _hasSubscribedToSceneChange = false;
        private float _copyOfFixedDeltaTime;

        #region MonoBehaviors
        protected override void Awake()
        {
            base.Awake();

            // Clone fixed delta time in case we need to restore or override it from anywhere in the game:
            this._copyOfFixedDeltaTime = Time.fixedDeltaTime;
        }

        // Start is called before the first frame update
        void Start()
        {
            RestrictMouseInputToGameWindow(shouldHideCursor: _hideMouseCursor, shouldConfineCursor: _confineMouseCursorToGameWindow);

            // Adding flag lock:
            if (!_hasSubscribedToSceneChange)
            {
                // Hook up listener to wipe out dropped game object references upon scene change:
                SceneManager.activeSceneChanged += ClearPauseManagerReference;
                _hasSubscribedToSceneChange = true;
                Debug.Log("MSG: Added GameManager subscriber to active scene changed event.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_currentPlayerInputSystem != null)
            {
                // Misc. managed inputs:
                if (_enableQuitGameOverrideOnTab)
                {
                    if (WasQuitOverrideTriggered())
                    {
                        QuitGame();
                    }
                }

                if (WasPauseTriggered() && IsAValidSceneForPausing())
                {
                    PauseController.TogglePausedGame();
                }
            }

            if (PauseUIController.GameIsPaused)
            {
                SetPlayerInputActionMapToUI();
                _gameHasBeenPaused = true;
            }
            else if (!PauseUIController.GameIsPaused && _gameHasBeenPaused)
            {
                SetPlayerInputActionMapToPlayer();
                _gameHasBeenPaused = false;
            }
        }

        void OnDisable()
        {
            if (_hasSubscribedToSceneChange)
            {
                // Removed subscriber if Singleton is disabled or queued for destruction:
                SceneManager.activeSceneChanged -= ClearPauseManagerReference;
                _hasSubscribedToSceneChange = false;
                Debug.Log("MSG: Removed GameManager subscriber to active scene changed event.");
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

        private void ClearPauseManagerReference(Scene current, Scene next)
        {
            if (PauseController != null)
            {
                PauseController = null;
            }

            // Reset to default action map:
            SetPlayerInputActionMapToPlayer();

            // Reset Pause time scaling and status:
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = this._copyOfFixedDeltaTime * Time.timeScale;
            PauseUIController.SetGameIsPaused(false);
        }

        #region Public Accessor(s)
        public PauseUIController GetGameManagedPauseController()
        {
            return PauseController;
        }
        #endregion

        #region Public Mutator(s)
        public void SetPlayerInputInstance(PlayerInputSystem newInputInstance)
        {
            _currentPlayerInputSystem = newInputInstance;
        }

        public void SetPlayerInputActionMapToUI()
        {
            if (_currentPlayerInputSystem != null)
            {
                _currentPlayerInputSystem.Player.Disable();
                _currentPlayerInputSystem.UI.Enable();
            }
        }

        public void SetPlayerInputActionMapToPlayer()
        {
            if (_currentPlayerInputSystem != null)
            {
                _currentPlayerInputSystem.UI.Disable();
                _currentPlayerInputSystem.Player.Enable();
            }

            // NOTE: You should call this each time we switch scenes since its the default action map.
        }

        public void SetPauseController(PauseUIController newController)
        {
            if (newController != null)
            {
                PauseController = newController;
            }
        }
        #endregion

        public bool IsMouseIgnoredOutsideGameWindow()
        {
            bool result = _ignoreMouseCursorOutsideGameWindow;
            return result;
        }

        public bool IsAValidSceneForPausing()
        {
            bool result = false;
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                result = true;
            }

            return result;
        }

        public bool WasQuitOverrideTriggered()
        {
            return _currentPlayerInputSystem.Player.QuitGameOverride.WasPressedThisFrame();
        }

        public bool WasPauseTriggered()
        {
            return (_currentPlayerInputSystem.Player.TogglePause.WasPressedThisFrame()
                    || 
                    _currentPlayerInputSystem.UI.TogglePause.WasPressedThisFrame());
        }

        public void QuitGame()
        {
            Debug.Log("MSG: Game Manager QUIT triggered.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // TODO: Handle WebGL shutdown case
        Application.Quit();
#endif
        }

        public void GameOver()
        {
            int numOfScenes = SceneManager.sceneCountInBuildSettings;
            // Assuming from the Build config:
            // The "Game Over" scene is always the last indexed scene:
            if (numOfScenes > 0)
            {
                SceneManager.LoadScene(numOfScenes - 1);
            }
            else
            {
                Debug.LogWarning("WARN: No scenes found in Build Settings. Please input them to correctly locate the Game Over scene.");
            }
        }
        #endregion
    }
}