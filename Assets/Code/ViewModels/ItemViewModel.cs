using UnityWeld.Binding;

[Binding]
public class ItemViewModel
{
    private IItem model;

    public ItemViewModel(IItem model)
    {
        this.model = model;
    }

    [Binding]
    public string Name => this.model.Name;
}
