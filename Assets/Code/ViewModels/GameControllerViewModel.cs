using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

/// <summary>
/// View-model for the game controller.
/// </summary>
public class GameControllerViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private GameController gameController;

    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public int Score => this.gameController.Score;

    [Binding]
    public void Click() => this.gameController.Click();

    // Use this for initialization
    private void Start()
    {
        this.gameController = GameController.Instance;
        this.gameController.OnScoreChanged += this.GameController_OnScoreChanged;
    }

    private void OnDestroy()
    {
        this.gameController.OnScoreChanged -= this.GameController_OnScoreChanged;
    }

    private void GameController_OnScoreChanged(object sender, GameController.ScoreChangedEventArgs eventArgs)
    {
        this.OnPropertyChanged(nameof(Score));
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
