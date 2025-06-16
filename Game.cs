using System;
using System.Diagnostics;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	public Enemy Enemy { get; private set; }

	private Stopwatch stopwatch;
	private Room chamber;

	private Room vaultchamber;

	private Room stairchamber;
	// private Room currentRoom;

	// Constructor
	public Game()
	{
		Enemy = new Enemy();
		parser = new Parser();
		player = new Player();
		CreateRooms();
		stopwatch = new Stopwatch();


	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{

		// Create the rooms
		Room startRoom = new Room("at the beginning of the sewers. With a brick wall behind you.");
		Room tunnel = new Room("in a a tunnel. It's dark and there's liquid dropping from the ceiling.");
		Room utilityRoom = new Room("in a utility room. There are some tools laying around.");
		Room abandonedSection = new Room("in an abandoned section of the sewers. It's packed full of dust and you can hear some strange noises.");
		Room storageRoom = new Room("in a storage room. There are some boxes and barrels laying around.");
		Room staireWell1 = new Room("walking down the stairs but you are blocked by trash.");
		Room staireWell2 = new Room("walking up the stairs but you are blocked by trash. Maybe you can get rid of it by drilling a big hole in it.");
		Room restroom = new Room("in a restroom. It smells awful, and the walls are covered in graffiti.");
		Room vault = new Room("standing in front of a huge vault. You wonder what is inside.");
		Room overFlowChamber = new Room("in the overflow chamber. The water is rising and you are drowning.(You're taking 5 damage per second)");
		chamber = overFlowChamber;
		vaultchamber = vault;
		stairchamber = staireWell2;



		// Initialise room exits
		startRoom.AddExit("east", tunnel);
		startRoom.AddExit("south", abandonedSection);
		startRoom.AddExit("west", utilityRoom);
		startRoom.AddExit("down", staireWell1);


		tunnel.AddExit("west", startRoom);
		tunnel.AddExit("east", overFlowChamber);
		overFlowChamber.AddExit("west", tunnel);


		utilityRoom.AddExit("east", startRoom);
		utilityRoom.AddExit("north", restroom);
		restroom.AddExit("south", utilityRoom);

		abandonedSection.AddExit("north", startRoom);
		abandonedSection.AddExit("east", storageRoom);
		abandonedSection.AddExit("west", vault);
		vault.AddExit("east", abandonedSection);

		storageRoom.AddExit("west", abandonedSection);
		storageRoom.AddExit("up", staireWell2);


		staireWell1.AddExit("up", startRoom);
		staireWell2.AddExit("down", storageRoom);


		// Create your Items here
		// ...
		// And add them to the Rooms
		// ...

		// startRoom game startRoom
		player.CurrentRoom = startRoom;
		Enemy.CurrentRoom = storageRoom;

		Item mousetail = new Item(3, "Why would you even want to pick up a mousetail? You still picked it up tho.");
		Item poopotion = new Item(5, "You picked up a bottle which looks like all the colors combined... You are wondering if u should drink it.");
		Item slingshot = new Item(2, "You picked up a slingshot. You can use it to shoot things.");
		Item drill = new Item(4, "Wow! this looks good. I wonder what I can do with this.");
		Item key = new Item(1, "KEYYYYYY!!!!");


		abandonedSection.Chest.Put("mousetail", mousetail);
		storageRoom.Chest.Put("poopotion", poopotion);
		utilityRoom.Chest.Put("slingshot", slingshot);
		vault.Chest.Put("drill", drill);
		restroom.Chest.Put("key", key);

	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		bool finished = false;
		// execute them until the player wants to quit.
		while (!finished)
		{

			stopwatch.Start();

			Command command = parser.GetCommand();
			OverFlowChamber(command);
			vault(command);



			finished = ProcessCommand(command);
			if (!player.IsAlive())
			//! if player is NOT alive (!) then finished is true
			{
				finished = true;
				Console.WriteLine("You died, noob!");
			}
			stopwatch.Reset();
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to 'The Sewers' ");
		Console.WriteLine("You fell into the sewers while working your regular 9-5. You feel scared so you take your water pistol out of your suitcase");
		Console.WriteLine("U smell a really nasty air hanging around this place, and you don’t feel comfortable at all… ");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());

	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				PrintLook();
				break;
			case "status":
				PrintStatus();
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				PrintUse(command);
				break;
			case "back":
				PrintBack(command);
				break;





		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around in the sewers besides the stinky water.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	private void PrintStatus()
	{
		Console.WriteLine("Your Health is: " + player.Health);
		Console.WriteLine("Your suitcase contains: " + player.Backpack.ShowInventory());
		Console.WriteLine("You are carrying: " + player.Backpack.TotalWeight() + "kg. You have " + player.Backpack.FreeWeight() + "kg free space.");
	}

	private void PrintUse(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Use what?");
			return;
		}

		string itemName = command.SecondWord;

		// Call the Use method in the Player class
		player.Use(itemName, Enemy);

		string target = command.HasThirdWord() ? command.ThirdWord : null; // Get the third word if it exists

		// Check if the player is in theExitHall and using the drill with the correct third word
		if (itemName == "drill" && target == "up")
		{
			// Check if the player has the drill in their inventory
			if (!player.Backpack.HasItem(itemName))
			{
				Console.WriteLine("You don't have a drill in your inventory.");
				return;
			}

			else if (player.CurrentRoom == stairchamber) // chamber is theExitHall
			{
				Console.WriteLine("As you drill ur way through the piles of trash. You can finally see your beloved 9-5 life again!");
				Console.WriteLine("Congratulations! Back to working for a boss until you're 70 years old.");
				Console.WriteLine("Press [Enter] to continue.");
				Environment.Exit(0); // End the game
			}


			else
			{
				Console.WriteLine("Doesn't seem to work here.");
			}
		}

	}

	private void PrintBack(Command command)
	{
		player.CurrentRoom = player.CurrentRoom.GetExit("up") ?? player.CurrentRoom.GetExit("down") ?? player.CurrentRoom.GetExit("north") ?? player.CurrentRoom.GetExit("south") ?? player.CurrentRoom.GetExit("east") ?? player.CurrentRoom.GetExit("west");
		Console.WriteLine("You are at the beginning of the sewers. With a brick wall behind you.");
	}


	private void PrintLook()
	{

		if (player.CurrentRoom == vaultchamber && !player.Backpack.HasItem("key"))
		{
			Console.WriteLine("The vault is locked. You need a key to enter"); // Only show this message
			return; // Prevent any other information from displaying
		}

		else if (player.CurrentRoom == vaultchamber && player.Backpack.HasItem("key"))
		{
			Console.WriteLine("You used the key to unlock the vault and step inside.");

		}
		Console.WriteLine("Items in the room: " + player.CurrentRoom.Chest.ShowInventory());

		// Check if there is an Enemy in the current room
		if (Enemy != null && Enemy.CurrentRoom == player.CurrentRoom && Enemy.IsAlive())
		{
			Console.WriteLine($"There is an {Enemy} standing in front of you. It has {Enemy.Health} health! Use your weapon to kill it");

		}


		else if (Enemy != null && Enemy.CurrentRoom == player.CurrentRoom && !Enemy.IsAlive())
		{
			Console.WriteLine($"There is a smelly dead {Enemy} lying here.");
		}


		else
		{
			Console.WriteLine("Enemies in the room: None");
		}

	}



	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{


		if (!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to " + direction + "!");
			return;
		}

		player.Damage(5);

		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());

	}

	//methods
	private void Take(Command command)
	{
		if (player.CurrentRoom == vaultchamber && !player.Backpack.HasItem("key"))
		{
			Console.WriteLine("The vault is locked."); // Prevent taking items
			return;
		}


		if (!command.HasSecondWord())
		{
			Console.WriteLine("Take what?");
			return;
		}

		string itemName = command.SecondWord;

		if (!player.TakeFromChest(itemName))
		{
			return;
		}

		// Add custom messages for specific items
		switch (itemName)
		{
			case "mousetail":
				Console.WriteLine("Why would you even want to pick up a mousetail? You still picked it up tho.");
				break;
			case "poopotion":
				Console.WriteLine("You picked up a bottle which looks like all the colors combined... You are wondering if you should drink it.");
				break;
			case "slingshot":
				Console.WriteLine("Dayum! I finally don't have to rely on my stupid water pistol anymore.");
				break;
			case "key":
				Console.WriteLine("Did you really shove your whole arm in that stinky toilet?");
				break;
			case "drill":
				Console.WriteLine("Wow! This looks good. I wonder what I can do with this.");
				break;
		}
	}



	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Drop what?");
			return;
		}

		string itemName = command.SecondWord;

		Item item = player.Backpack.Get(itemName);
		if (item != null)
		{
			player.CurrentRoom.Chest.Put(itemName, item);
			Console.WriteLine($"You dropped the {itemName}.");
		}
		else
		{
			Console.WriteLine($"You don't have a {itemName} to drop.");
		}
	}

	private void OverFlowChamber(Command command)
	{
		// Console.WriteLine("aaaaa");
		if (player.CurrentRoom == chamber) // Use a proper identifier
		{

			stopwatch.Stop();
			int s = stopwatch.Elapsed.Seconds;

			for (int i = 0; i < s; i++)
			{
				player.Damage(5);
			}
			Console.WriteLine("You're struggling in the flooded chamber!");


			if (!player.IsAlive())
			{
				Console.WriteLine("You drowned in the overflow chamber!");
				// break;

			}
		}

	}

	private void vault(Command command)
	{
		if (player.CurrentRoom == vaultchamber)
		{


		}
	}



}