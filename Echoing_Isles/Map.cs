// Map.cs
using System.Collections.Generic;

public class Map
{
    private Dictionary<(int, int), Location> _locations = new Dictionary<(int, int), Location>();

    public void AddLocation(Location location)
    {
        _locations.Add((location.X, location.Y), location);
    }

    public Location? GetLocation(int x, int y)
    {
        if (_locations.TryGetValue((x, y), out Location location))
        {
            return location;
        }
        return null;
    }
    public bool IsLocation(int x, int y)
    {
      return _locations.ContainsKey((x,y));
    }
}