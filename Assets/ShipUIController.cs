using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Bsting.Ship.Managers;

public class ShipUIController : MonoBehaviour
{
    // Properties:
    [field: SerializeField] public TextMeshProUGUI ScoreText { get; private set; } = null;
    [field: SerializeField] public GameObject HealthBarGroup { get; private set; } = null;

    // Fields:
    [Header("Player Score Fields")]
    [SerializeField] private int _scoreIncrementRate = 50;

    // Private var's:
    private int _currentScore = 0;
    private Stack<GameObject> _stackOfHP = new Stack<GameObject>();
    private bool _isHealthBarBeingReset = false;

    #region MonoBehaviors
    void Awake()
    {
        // Do forced resets on boot of this controller, for prototyping the UI:
        ResetPlayerScore();
        ResetPlayerStackOfHealth();
    }

    // Update is called once per frame
    void Update()
    {
        // Do checks related to player score and health:
        ChecktoUpdateScore();
        CheckForGameOver();
    }
    #endregion

    #region Helper Method(s)
    private void ChecktoUpdateScore()
    {
        if (ScoreText != null)
        {
            string displayedScore = ScoreText.text;
            int parsedDisplayedScore = int.Parse(displayedScore);
            if (parsedDisplayedScore != _currentScore)
            {
                ScoreText.SetText(_currentScore.ToString());
            }
        }
    }

    private void CheckForGameOver()
    {
        // See if health has reached 0 (not from HP resets):
        if (!_isHealthBarBeingReset && _stackOfHP.Count <= 0)
        {
            // Call on GameManager instance to change the scene to Game Over:
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    private void AddAllChildHPToHealthStack()
    {
        if (HealthBarGroup != null)
        {
            int numOfHealthPoints = HealthBarGroup.transform.childCount;

            for (int index = 0; index < numOfHealthPoints; index++)
            {
                // Get the current child game object under the ship controller:
                GameObject currentHPHeart = transform.GetChild(index).gameObject;

                // Also make each visible here too (set alpha to 1):
                SetAlphaTransparencyOfHP(currentHPHeart, 1f);

                _stackOfHP.Push(currentHPHeart);
            }
        }
    }

    private void SetAlphaTransparencyOfHP(GameObject heartObj, float newTransparency)
    {
        if (heartObj != null)
        {
            // Change the heart icon's alpha to the new alpha level (0 to 1 scale by default): 
            Image heartIcon = heartObj.GetComponent<Image>();

            if (heartIcon != null)
            {
                heartIcon.color = new Color(heartIcon.color.r, heartIcon.color.b, heartIcon.color.g, newTransparency);
            }
        }
    }

    public void ResetPlayerScore()
    {
        _currentScore = 0;
        Debug.Log("MSG: the player's score has been reset.");
    }

    public void UptickPlayerScore()
    {
        _currentScore += _scoreIncrementRate;
    }

    public void ResetPlayerStackOfHealth()
    {
        // Update flag first so we don't accidentally trigger Game Over:
        _isHealthBarBeingReset = true;

        // Clear out the stack's contents first:
        _stackOfHP.Clear();
        // Then push all the heart objects back in:
        AddAllChildHPToHealthStack();

        // Re-close flag to say that the reset process is done:
        _isHealthBarBeingReset = false;
    }

    public void RemoveHPFromPlayer()
    {
        // Make a cache for popped heart:
        GameObject poppedHP = null;

        // Try to pop the HP heart:
        _stackOfHP.TryPop(out poppedHP);

        if (poppedHP != null)
        {
            // Make it invisible via setting its Alpha to nothing:
            SetAlphaTransparencyOfHP(poppedHP, 0f);
        }

        // NOTE: you should hook this method up to an OnCollision check with ship + asteroid
    }
    #endregion
}
