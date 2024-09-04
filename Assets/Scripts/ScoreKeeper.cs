using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreKeeper : MonoBehaviour
{
    public int Score = 0;
    public int Best = 0;
    public static Action<int, int> OnUpdateScoreEvent;
    public static Action UpdateScore;
    private void OnEnable()
    {
        UpdateScore += ScoreUpdate;
        BallSpawner.OnCombineEvent += IncreaseScore;
        GameStateManager.GameRestart += ResetScore;
    }
    private void OnDisable()
    {
        UpdateScore -= ScoreUpdate;
        BallSpawner.OnCombineEvent -= IncreaseScore;
        GameStateManager.GameRestart -= ResetScore;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            Best = PlayerPrefs.GetInt("BestScore");
        }
        else
        {
            SetNewBest(0);
        }
        ResetScore();
    }

    private void SetNewBest(int score)
    {
        PlayerPrefs.SetInt("BestScore", score);
        Best = score;
    }

    private void ResetScore()
    {
        Score = 0;
        ScoreUpdate();
    }
    private void ScoreUpdate()
    {
        OnUpdateScoreEvent?.Invoke(Score, Best);
    }
    private void IncreaseScore(int points)
    {
        Score += points;
        if (Score > Best)
        {
            SetNewBest((int)Score);
        }
        ScoreUpdate();
    }
}
