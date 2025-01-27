using System.Collections.Generic;

public class Player
{
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;
    public List<Item> Inventory { get; } = new List<Item>();
     public void AddItem(Item item)
    {
        Inventory.Add(item);
    }
     public void RemoveItem(Item item)
    {
        Inventory.Remove(item);
    }
      public bool HasItem(int itemId)
    {
           foreach (Item item in Inventory)
            {
                if(item.Id == itemId)
                {
                   return true;
                }
            }
         return false;
    }
}