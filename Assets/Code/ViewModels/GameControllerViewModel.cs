using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

/// <summary>
/// View-model for the game controller.
/// </summary>
[Binding]
public class GameControllerViewModel : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public int Score => GameController.Instance.Score;

    [Binding]
    public void Click() => GameController.Instance.Click();

    // Use this for initialization
    private void Start()
    {
        GameController.Instance.OnScoreChanged += this.GameController_OnScoreChanged;
    }

    private void OnDestroy()
    {
        GameController.Instance.OnScoreChanged -= this.GameController_OnScoreChanged;
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
