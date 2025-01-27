using System.Collections.Generic;
public class Location
{
    public string Name { get; }
    public string Description { get; }
    public NPC? NPC { get; set; }
    public List<Item> Items { get; } = new List<Item>();

    public int X { get; }
    public int Y { get; }
    public string OnEnterMessage { get; set; } = "";
    public Location(string name, string description, int x, int y)
    {
        Name = name;
        Description = description;
        X = x;
        Y = y;

    }
    public Location(string name, string description, int x, int y, string onEnterMessage)
    {
        Name = name;
        Description = description;
        X = x;
        Y = y;
        OnEnterMessage = onEnterMessage;

    }
    public void AddItem(Item item)
    {
        Items.Add(item);
    }
    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }

}