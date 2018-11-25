/// <summary>
/// Allows us to buy items.
/// </summary>
public class ItemShop
{
    public static readonly int DoubleClickerPrice = 10;

    public bool CanPurchaseDoubleClicker =>
        this.gameController.Score >= DoubleClickerPrice;

    private readonly GameController gameController;

    public ItemShop(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void PurchaseDoubleClicker()
    {
        this.gameController.BuyItem(DoubleClickerPrice, new DoubleClickerItem());
    }
}
