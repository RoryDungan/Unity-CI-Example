using System;
using System.ComponentModel;
using UnityWeld.Binding;

[Binding]
public class ItemShopViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly ItemShop itemShop;

    // Needed to subscribe to score changed events so that we know when
    // we can purchase new items
    private readonly GameController gameController;

    public ItemShopViewModel(GameController gameController)
    {
        this.gameController = gameController;
        this.itemShop = gameController.ItemShop;
        this.gameController.OnScoreChanged += this.GameController_OnScoreChanged;
    }

    [Binding]
    public int DoubleClickerPrice =>
        ItemShop.DoubleClickerPrice;

    [Binding]
    public bool CanPurchaseDoubleClicker =>
        this.itemShop.CanPurchaseDoubleClicker;

    public event PropertyChangedEventHandler PropertyChanged;

    public void Dispose()
    {
        this.gameController.OnScoreChanged -= this.GameController_OnScoreChanged;
    }

    private void GameController_OnScoreChanged(object sender, GameController.ScoreChangedEventArgs eventArgs)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(
                this,
                new PropertyChangedEventArgs(nameof(CanPurchaseDoubleClicker))
            );
        }
    }

    [Binding]
    public void PurchaseDoubleClicker() =>
        this.itemShop.PurchaseDoubleClicker();
}
