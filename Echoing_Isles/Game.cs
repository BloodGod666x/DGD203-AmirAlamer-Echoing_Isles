// Game.cs
using System;
using System.Threading;

public class Game
{
    private Map _map;
    private Player _player;
    private bool _gameRunning = true;
    private bool _hasGameEnded = false;
    public Game()
    {
        _map = new Map();
        _player = new Player();

        // create locations
        Location beach = new Location("Sandy Beach", "Golden sand stretches along the shore, with gentle waves lapping at the edge.", 0, 0);
        Location forestEdge = new Location("Forest Edge", "The dense forest looms, dark and mysterious, blocking the way north.", 0, 1, "The air feels colder as you move towards the forest.");
        Location oldCabin = new Location("Old Cabin", "A dilapidated cabin stands, with broken windows and a weathered door.", 0, 2, "You can feel the history within the old walls.");
        Location mysticalCave = new Location("Mystical Cave", "A dark cave opening is before you, with unkown depths.", 1, 1, "A strange glow is coming from within.");

        // add locations to map
        _map.AddLocation(beach);
        _map.AddLocation(forestEdge);
        _map.AddLocation(oldCabin);
        _map.AddLocation(mysticalCave);

        // Add items
        beach.AddItem(new Item("Rusty Key", "A worn, old key, rusted by the sea air.", 1, true)); // id 1
        mysticalCave.AddItem(new Item("Glowing Stone", "A warm, pulsing stone, glowing faintly.", 2));
        // Add NPCs
        oldCabin.NPC = new NPC("Old Hermit", "Welcome stranger... Have you come to seek the answers to the echoing isles?");

    }
    public void Run()
    {
        while (_gameRunning)
        {
             if(_hasGameEnded)
            {
                EndMenu();
            }
            else
            {
                MainMenu();
            }
           
        }
    }
    private void MainMenu()
    {
        Console.Clear();
        Console.WriteLine("==== Echoing Isles ====");
        Console.WriteLine("1. New Game");
        Console.WriteLine("2. Load Game");
        Console.WriteLine("3. Credits");
        Console.WriteLine("4. Exit");
        Console.Write("Choose an option: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                StartNewGame();
                break;
            case "2":
                LoadGame();
                break;
            case "3":
                DisplayCredits();
                break;
            case "4":
                _gameRunning = false;
                break;
            default:
                Console.WriteLine("Invalid Choice. Please try again.");
                Thread.Sleep(1000);
                break;
        }
    }
        private void EndMenu()
    {
        Console.Clear();
        Console.WriteLine("==== Echoing Isles ====");
         Console.WriteLine("The Echoing Isles have been explored.");
        Console.WriteLine("1. New Game");
        Console.WriteLine("2. Credits");
        Console.WriteLine("3. Exit");
        Console.Write("Choose an option: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                _hasGameEnded = false;
                 StartNewGame();
                break;
            case "2":
                DisplayCredits();
                break;
            case "3":
                _gameRunning = false;
                break;
            default:
                Console.WriteLine("Invalid Choice. Please try again.");
                Thread.Sleep(1000);
                break;
        }
    }

    private void StartNewGame()
    {
        _player = new Player();
        GameLoop();
    }
    private void LoadGame()
    {
        (Player loadedPlayer, bool loaded) = SaveLoad.LoadGame();
        if(loaded)
        {
            _player = loadedPlayer;
            GameLoop();
        }
        else
        {
           Console.WriteLine("No save file found, starting new game");
           Thread.Sleep(1000);
           StartNewGame();
        }
    }

    private void GameLoop()
    {
        while (!_hasGameEnded)
        {
            Console.Clear();
            Location currentLocation = _map.GetLocation(_player.X, _player.Y);
             if(currentLocation != null)
            {
                Console.WriteLine($"--- {currentLocation.Name} ---");
                Console.WriteLine(currentLocation.Description);
                if(currentLocation.OnEnterMessage != "")
                    Console.WriteLine(currentLocation.OnEnterMessage);
                
                if (currentLocation.Items.Count > 0)
                {
                    Console.WriteLine("Items here:");
                   for(int i = 0; i < currentLocation.Items.Count; i++)
                    {
                        Console.WriteLine($"- {currentLocation.Items[i].Name}");
                    }
                    Console.WriteLine("Type 'pickup <item name>' to pick up an item.");
                }
                if (currentLocation.NPC != null)
                {
                    Console.WriteLine($"{currentLocation.NPC.Name} is here.");
                    Console.WriteLine("Type 'talk' to talk to them");
                }
                 Console.WriteLine("--------------------");
                  Console.WriteLine("Type 'move <direction (north, east, south, west)>' to move or type 'inventory' to check items");
                 Console.WriteLine("Type 'save' to save the game");
                ProcessInput(currentLocation);
            }
            else
            {
                Console.WriteLine("You are in the void. (There is no map here)");
                 Console.WriteLine("Type 'move <direction (north, east, south, west)>' to move");
                ProcessInput(currentLocation);
            }

        }
    }
    private void ProcessInput(Location? currentLocation)
    {
         Console.Write("> ");
        string input = Console.ReadLine().ToLower();

         if (input.StartsWith("pickup "))
        {
            PickupItem(input.Substring(7), currentLocation);
        }
        else if (input.StartsWith("move "))
        {
            MovePlayer(input.Substring(5));
        }
        else if(input == "inventory")
        {
           ShowInventory();
        }
        else if (input == "talk")
        {
            TalkToNPC(currentLocation);
        }
        else if (input == "save")
        {
          SaveGame();
        }
        else
        {
           Console.WriteLine("Invalid input.");
        }

    }
    private void PickupItem(string itemName, Location? currentLocation)
    {
      if(currentLocation != null)
      {
          Item? itemToRemove = null;
          foreach (Item item in currentLocation.Items)
          {
              if (item.Name.ToLower() == itemName.Trim())
               {
                 itemToRemove = item;
                 break;
              }
         }
          if (itemToRemove != null)
          {
              _player.AddItem(itemToRemove);
              currentLocation.RemoveItem(itemToRemove);
               Console.WriteLine($"You picked up {itemToRemove.Name}.");
          }
          else
          {
              Console.WriteLine("No such item here.");
          }
      }
      else
      {
        Console.WriteLine("No item can be picked up here");
      }

    }
    private void MovePlayer(string direction)
    {
        switch (direction.Trim())
        {
            case "north":
                _player.Y++;
                break;
            case "south":
                _player.Y--;
                break;
            case "east":
                _player.X++;
                break;
            case "west":
                _player.X--;
                break;
            default:
                Console.WriteLine("Invalid direction.");
                break;
        }
         if(_player.X == 1 && _player.Y == 2 && _player.HasItem(1))
         {
           EndGame();
         }
    }
      private void ShowInventory()
    {
        if(_player.Inventory.Count == 0)
        {
           Console.WriteLine("Your inventory is empty");
        }
       else
       {
         Console.WriteLine("Inventory:");
          foreach (Item item in _player.Inventory)
            {
                Console.WriteLine($" - {item.Name}: {item.Description}");
            }
       }
        Console.WriteLine("Press any key to continue.");
         Console.ReadKey();
    }
    private void TalkToNPC(Location? currentLocation)
    {
       if(currentLocation != null && currentLocation.NPC != null)
        {
             Console.WriteLine($"{currentLocation.NPC.Name} says: {currentLocation.NPC.Dialogue}");
        }
        else
        {
          Console.WriteLine("There is no one to talk to");
        }
      Console.WriteLine("Press any key to continue.");
         Console.ReadKey();
    }
    private void EndGame()
    {
          _hasGameEnded = true;
    }
      private void DisplayCredits()
    {
        Console.Clear();
        Console.WriteLine("==== Credits ====");
        Console.WriteLine("Game Created by: AmirAlamer");
        Console.WriteLine("All the game concept was created by me");
        Console.WriteLine("Thank you for playing Echoing Isles!");
        Console.WriteLine("Press any key to return to menu");
        Console.ReadKey();
    }
    private void SaveGame()
    {
        SaveLoad.SaveGame(_player);
         Console.WriteLine("Game saved.");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }
}