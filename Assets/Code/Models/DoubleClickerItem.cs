public class DoubleClickerItem : IItem
{
    public int HandleClick()
    {
        // Give 1 extra point per click.
        return 1;
    }

    public int Update(float currentTime)
    {
        // This item does not generate any score over time.
        return 0;
    }
}
