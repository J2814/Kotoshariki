using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreui : MonoBehaviour, IGameStateResonder
{
    public Text scoreText;
    private void OnEnable()
    {
        ScoreKeeper.OnUpdateScoreEvent += UpdateScore;
        ScoreKeeper.UpdateScore?.Invoke();
    }
    private void OnDisable()
    {
        ScoreKeeper.OnUpdateScoreEvent -= UpdateScore;
    }

    private void UpdateScore(int score, int best)
    {
        scoreText.text = "Score:\n" + score.ToString() + "\nBest:\n" + best.ToString();
    }

    public void RespondToGameState(GameStateManager.GameState state)
    {
        ScoreKeeper.UpdateScore?.Invoke();
    }
}
