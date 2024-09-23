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

        void OnEnable()
        {
            TryToStartDefaultUIMenu();
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
                    Debug.LogWarning("WARN: No UI menus provided in UI Manager's list. No UI-disabling action has been taken.");
                }

                // Then enable the one we want enabled by default:
                if (UIMenuToEnableOnBoot != null)
                {
                    UIMenuToEnableOnBoot.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("WARN: No default UI menu has been specified. No UI-enabling action has been taken.");
                }
            }
        }
    }
}