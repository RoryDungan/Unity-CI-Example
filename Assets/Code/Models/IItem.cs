/// <summary>
/// Base interface for all items.
/// </summary>
public interface IItem
{
    /// <summary>
    /// Allow the item to modify how much score we receive from click events.
    ///
    /// This method is called each time a click event happens and the result
    /// from all items is added together and added to the current score.
    /// </summary>
    int HandleClick();

    /// <summary>
    /// Allow the item to passively generate score.
    /// </summary>
    int Update(float currentTime);
}
