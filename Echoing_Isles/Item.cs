// Item.cs
public class Item
{
    public string Name { get; }
    public string Description { get; }
    public bool IsUsable { get; } // can this item be used?
    public int Id { get; }
    public Item(string name, string description, int id, bool isUsable = false)
    {
        Name = name;
        Description = description;
        IsUsable = isUsable;
        Id = id;

    }
}