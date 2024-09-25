using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bsting.Ship.Managers
{
    public class UIManager : Manager<UIManager>
    {
        // Public UI Manager properties:
        [field: SerializeField] public bool EnableDefaultUIBehavior { get; private set; } = true;
        [field: SerializeField] public GameObject UIMenuToEnableOnBoot { get; private set; } = null;
        [field: SerializeField] public List<GameObject> ListOfUIMenus { get; private set; } = new List<GameObject>();

        private const int _MAIN_MENU_INDEX = 0;
        private bool _hasSubscribedToSceneChange = false;

        void OnEnable()
        {
            TryToStartDefaultUIMenu();
        }

        void OnDisable()
        {
            if (_hasSubscribedToSceneChange)
            {
                // Removed subscriber if Singleton is disabled or queued for destruction:
                SceneManager.activeSceneChanged -= UpdateListOfCurrentMenus;
                _hasSubscribedToSceneChange = false;
                Debug.Log("MSG: Removed UIManager subscriber to active scene changed event.");
            }
        }

        private void Start()
        {
            /* --- Setup a one-shot default behavior with UI Manager --- */
            // (clear the UI serialized property fields after scene change happens)

            // Adding flag lock:
            if (!_hasSubscribedToSceneChange)
            {
                // Subscribe the clear and set menus method to the activeSceneChanged event:
                SceneManager.activeSceneChanged += UpdateListOfCurrentMenus;
                _hasSubscribedToSceneChange = true;
                Debug.Log("MSG: Added UIManager subscriber to active scene changed event.");
            }
        }

        public void PlayMainGame()
        {
            int queuedBuildIndex = (SceneManager.GetActiveScene().buildIndex) + 1;

            if (IsSceneBuildIndexInRange(queuedBuildIndex))
            {
                // Load the next queued scene index (the one after the main menu; should be "Main Scene"):
                SceneManager.LoadScene(queuedBuildIndex);
            }
        }

        public void GoToMainMenu()
        {
            if (!IsBuildIndexMainMenu(SceneManager.GetActiveScene().buildIndex))
            {
                SceneManager.LoadScene(_MAIN_MENU_INDEX);
            }
        }

        public void ForceQuitGame()
        {
            // Check to see if we can centralize the Quit through Game Manager:
            if (GameManager.Instance != null)
            {
                GameManager.Instance.QuitGame();
            }
            else
            {
                // Do same quit process but without Game Manager's help:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
        // TODO: Handle WebGL shutdown case
        Application.Quit();
#endif
            }
        }

        private bool IsSceneBuildIndexInRange(int thisBuildIndex)
        {
            if (thisBuildIndex < SceneManager.sceneCountInBuildSettings)
            {
                return true;
            }
            else
            {
                Debug.LogWarning("WARN: Requested scene build index is out of range. Please revise.");
                return false;
            }
        }

        private bool IsBuildIndexMainMenu(int thisBuildIndex)
        {
            if (thisBuildIndex > 0)
            {
                // It's a level beyond the main menu spot, so no:
                return false;
            }
            else
            {
                // It is scene index 0 or less; so yes:
                return true;
            }
        }

        private void DisableAllListedUIMenus()
        {
            // Go through each menu and disable it (make it invisible, essentially):
            foreach (GameObject menu in ListOfUIMenus)
            {
                menu.SetActive(false);
            }
        }

        private void TryToStartDefaultUIMenu()
        {
            if (EnableDefaultUIBehavior)
            {
                // Go through and disable all UI Menus listed first:
                if (ListOfUIMenus.Count > 0)
                {
                    DisableAllListedUIMenus();
                }
                else
                {
                    Debug.Log("MSG: No UI menus provided in UI Manager's list. No UI-disabling action has been taken.");
                }

                // Then enable the one we want enabled by default:
                if (UIMenuToEnableOnBoot != null)
                {
                    UIMenuToEnableOnBoot.SetActive(true);
                }
                else
                {
                    Debug.Log("MSG: No default UI menu has been specified. No UI-enabling action has been taken.");
                }
            }
        }

        private void UpdateListOfCurrentMenus(Scene current, Scene next)
        {
            if (ListOfUIMenus.Count > 0)
            {
                // Verified current is no longer the active scene; proceed to clear:
                if (SceneManager.GetActiveScene().name != current.name)
                {
                    ListOfUIMenus.Clear();
                    UIMenuToEnableOnBoot = null;
                    Debug.Log("MSG: UI Manager has cleared its old menu references from last scene.");
                }
            }

            if (EnableDefaultUIBehavior)
            {
                // Now that fields are reset, disable the default startup:
                if (!IsBuildIndexMainMenu(next.buildIndex))
                {
                    EnableDefaultUIBehavior = false;
                }
            }
        }
    }
}