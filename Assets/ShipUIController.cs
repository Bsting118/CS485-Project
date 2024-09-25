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

    // Fields:
    [Header("Player Score Fields")]
    [SerializeField] private int _scoreIncrementRate = 50;

    // Private var's:
    private int _currentScore = 0;

    #region MonoBehaviors
    void Awake()
    {
        ResetPlayerScore();
    }

    // Update is called once per frame
    void Update()
    {
        ChecktoUpdateScore();
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

    public void ResetPlayerScore()
    {
        _currentScore = 0;
        Debug.Log("MSG: the player's score has been reset.");
    }

    public void UptickPlayerScore()
    {
        _currentScore += _scoreIncrementRate;
    }
    #endregion
}
