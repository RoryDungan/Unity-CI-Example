using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the global state of the game.
/// </summary>
public class GameController : Singleton<GameController>
{
    private int score;

    public int Score
    {
        get
        {
            return score;
        }
        private set
        {
            score = value;

            // Notify score changed
            if (OnScoreChanged != null)
            {
                OnScoreChanged(this, new ScoreChangedEventArgs(score));
            }
        }
    }

    public GameController()
    {
    }

    public event EventHandler<ScoreChangedEventArgs> OnScoreChanged;

    public class ScoreChangedEventArgs : EventArgs
    {
        public readonly int score;

        public ScoreChangedEventArgs(int newScore)
        {
            this.score = newScore;
        }
    }
}
