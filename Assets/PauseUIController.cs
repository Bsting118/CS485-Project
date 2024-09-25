using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bsting.Ship.Managers;

public class PauseUIController : MonoBehaviour
{
    // Default Pause Menu passed; if null, this script will find the first child tagged PauseMenu to use:
    [field: SerializeField] public GameObject PauseMenuToggle { get; private set; } = null;
    // Use this public var, mainly with ship's UI overlay, to disable non-paused stuff:
    public static bool GameIsPaused = false;
    // Used for storing Unity's fixed delta time:
    private float _fixedDeltaTime;

    #region MonoBehaviors
    void Awake()
    {
        // Make a hard copy of fixed delta time when being loaded
        this._fixedDeltaTime = Time.fixedDeltaTime; 

        // On startup, see if we need a PauseMenuToggle assigned or if we're good to go:
        if (PauseMenuToggle != null)
        {
            PauseMenuToggle.SetActive(false);
        }
        else
        {
            // Fetch it first then go through the visibility setter:
            GetChildPauseToggle();
            PauseMenuToggle.SetActive(false);
        }
    }

    void Update()
    {
        // Check in on Game Manager to see if it needs its Pause Controller connected back in:
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.GetGameManagedPauseController() == null)
            {
                GameManager.Instance.SetPauseController(this);
            }
        }
    }
    #endregion

    #region Helper Method(s)
    private void Resume()
    {
        if (PauseMenuToggle != null)
        {
            // Make the main pause overlay invisible:
            PauseMenuToggle.SetActive(false);
        }
        else
        {
            // Fetch it first then go through the visibility setter:
            GetChildPauseToggle();
            PauseMenuToggle.SetActive(false);
        }
        // Set the time to normal runtime for physics engine:
        Time.timeScale = 1.0f;
        // Adjust fixed delta time according to timescale:
        Time.fixedDeltaTime = this._fixedDeltaTime * Time.timeScale;
        // Update flag status variable:
        GameIsPaused = false;
    }

    private void Pause()
    {
        if (PauseMenuToggle != null)
        {
            // Make the main pause overlay visible:
            PauseMenuToggle.SetActive(true);
        }
        else
        {
            // Fetch it first then go through the visibility setter:
            GetChildPauseToggle();
            PauseMenuToggle.SetActive(true);
        }
        // Freeze time by making its factor = 0 (X * 0 = 0)
        Time.timeScale = 0f;
        // Adjust fixed delta time according to timescale:
        Time.fixedDeltaTime = this._fixedDeltaTime * Time.timeScale;
        // Update status variable:
        GameIsPaused = true;
    }

    private void GetChildPauseToggle()
    {
        if (PauseMenuToggle == null)
        {
            // Refresh it:
            for (int index = 0; index < transform.childCount; index++)
            {
                // Get the current child game object under the ship controller:
                GameObject currentChild = transform.GetChild(index).gameObject;

                // Check if its a pause toggle, and if so, assign it and break out:
                if (currentChild.CompareTag("PauseToggle"))
                {
                    PauseMenuToggle = currentChild;
                    break;
                }
            }
        }
    }
    #endregion

    public float GetStoredFixedDeltaTime()
    {
        return this._fixedDeltaTime;
    }

    #region Main Public Method(s)
    public void TogglePausedGame()
    {
        // If we are paused, resume the game:
        if (GameIsPaused)
        {
            Resume();
        }
        // Else, pause the game:
        else
        {
            Pause();
        }
    }

    public void TryToForceResume()
    {
        // We must be already paused to trigger a resume:
        if (GameIsPaused)
        {
            Resume();
        }
    }
    #endregion
}
