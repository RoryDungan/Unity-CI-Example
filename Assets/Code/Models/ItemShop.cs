/// <summary>
/// Allows us to buy items.
/// </summary>
public class ItemShop
{
    public static readonly int DoubleClickerPrice = 10;

    public static readonly int AutoClickerPrice = 50;

    public bool CanPurchaseDoubleClicker =>
        this.gameController.Score >= DoubleClickerPrice;

    public bool CanPurchaseAutoClicker =>
        this.gameController.Score >= AutoClickerPrice;

    private readonly GameController gameController;

    public ItemShop(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void PurchaseDoubleClicker()
    {
        this.gameController.BuyItem(DoubleClickerPrice, new DoubleClickerItem());
    }

    public void PurchaseAutoClicker()
    {
        this.gameController.BuyItem(AutoClickerPrice, new AutoClickerItem());
    }
}
