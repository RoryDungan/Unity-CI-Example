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

    private ItemShopViewModel itemShop;

    [Binding]
    public ItemShopViewModel ItemShop
    {
        get
        {
            if (this.itemShop == null)
            {
                this.itemShop = new ItemShopViewModel(this.gameController);
            }
            return this.itemShop;
        }
    }

    private BoundObservableList<ItemViewModel, IItem> items;

    [Binding]
    public BoundObservableList<ItemViewModel, IItem> Items
    {
        get
        {
            if (this.items == null)
            {
                this.items = new BoundObservableList<ItemViewModel, IItem>(
                    this.gameController.Items,
                    item => new ItemViewModel(item)
                );
            }
            return this.items;
        }
    }

    // Use this for initialization
    private void Awake()
    {
        Assert.IsNotNull(gameController);

        this.gameController.OnScoreChanged += this.GameController_OnScoreChanged;
    }

    private void OnDestroy()
    {
        this.gameController.OnScoreChanged -= this.GameController_OnScoreChanged;
        if (this.items != null)
        {
            this.items.Dispose();
        }
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
