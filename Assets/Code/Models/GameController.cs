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

    /// <summary>
    /// Current score value. In this case, the number of cookies that have
    /// been collected.
    /// </summary>
    public int Score
    {
        get
        {
            return score;
        }
        private set
        {
            this.score = value;

            // Notify score changed
            if (this.OnScoreChanged != null)
            {
                this.OnScoreChanged(this, new ScoreChangedEventArgs(value));
            }
        }
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

    public GameController()
    {
    }

    /// <summary>
    /// "Click" to collect a cookie.
    /// </summary>
    public void Click()
    {
        this.Score++;
    }
}
