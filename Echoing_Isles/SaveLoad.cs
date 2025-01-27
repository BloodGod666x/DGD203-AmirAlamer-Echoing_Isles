// SaveLoad.cs
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;


public static class SaveLoad
{
   
    public static void SaveGame(Player player, string saveFile = "savegame.json")
    {
        SaveData data = new SaveData { PlayerX = player.X, PlayerY = player.Y, Inventory = player.Inventory };
        string json = JsonSerializer.Serialize(data);
        File.WriteAllText(saveFile, json);
    }


    public static (Player, bool) LoadGame(string saveFile = "savegame.json")
    {
       if (File.Exists(saveFile))
       {
           string json = File.ReadAllText(saveFile);
           SaveData? data = JsonSerializer.Deserialize<SaveData>(json);

          if(data != null)
          {
            Player player = new Player();
            player.X = data.PlayerX;
            player.Y = data.PlayerY;
            player.Inventory.AddRange(data.Inventory);
            return (player, true);
          }
          return (new Player(), false);


       }
        return (new Player(), false);
    }

   
    private class SaveData
    {
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }
        public List<Item> Inventory { get; set; } = new List<Item>();
    }
}