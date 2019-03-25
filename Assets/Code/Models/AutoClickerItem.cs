/// <summary>
/// Item that "clicks" automatically once per second.
/// </summary>
public class AutoClickerItem : IItem
{
    private float lastClickTime = 0f;

    private static readonly float ClickInterval = 1f;

    public string Name => "Auto-clicker";

    public int HandleClick()
    {
        return 0;
    }

    public int Update(float currentTime)
    {
        if (currentTime > this.lastClickTime + ClickInterval)
        {
            this.lastClickTime = currentTime;
            return 32;
        }
        return 0;
    }
}
