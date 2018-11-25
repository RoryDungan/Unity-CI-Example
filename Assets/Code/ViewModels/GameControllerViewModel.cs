using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityWeld.Binding;

/// <summary>
/// View-model for the game controller.
/// </summary>
[Binding]
public class GameControllerViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameController gameController;

    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public int Score => this.gameController.Score;

    [Binding]
    public void Click() => this.gameController.Click();

    [Binding]
    public ItemShopViewModel ItemShop { get; private set; }

    // Use this for initialization
    private void Awake()
    {
        Assert.IsNotNull(gameController);

        this.gameController.OnScoreChanged += this.GameController_OnScoreChanged;
        this.ItemShop = new ItemShopViewModel(this.gameController);
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
