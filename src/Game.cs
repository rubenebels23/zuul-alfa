using System;
using System.Diagnostics;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	private Stopwatch stopwatch;
	private Room chamber;
	// private Room currentRoom;

	// Constructor
	public Game()
	{
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
		Room stareWell1 = new Room("walking down the stairs but you are blocked by trash.");
		Room stareWell2 = new Room("walking up the stairs but you are blocked by trash.");
		Room overFlowChamber = new Room("in the overflow chamber. The water is rising and you are drowning.(You're taking 5 damage per second)");
		chamber = overFlowChamber;

		// Initialise room exits
		startRoom.AddExit("east", tunnel);
		startRoom.AddExit("south", abandonedSection);
		startRoom.AddExit("west", utilityRoom);
		startRoom.AddExit("down", stareWell1);


		tunnel.AddExit("west", startRoom);
		tunnel.AddExit("east", overFlowChamber);
		overFlowChamber.AddExit("west", tunnel);


		utilityRoom.AddExit("east", startRoom);

		abandonedSection.AddExit("north", startRoom);
		abandonedSection.AddExit("east", storageRoom);

		storageRoom.AddExit("west", abandonedSection);
		storageRoom.AddExit("up", stareWell2);


		stareWell1.AddExit("up", startRoom);
		stareWell2.AddExit("down", storageRoom);


		// Create your Items here
		// ...
		// And add them to the Rooms
		// ...

		// startRoom game startRoom
		player.CurrentRoom = startRoom;
		Item mousetail = new Item(1, "Why would you even want to pick up a mousetail? You still picked it up tho.");
		Item poopotion = new Item(2, "You picked up a bottle which looks like all the colors combined... You are wondering if u should drink it.");
		Item slingshot = new Item(1, "You picked up a slingshot. You can use it to shoot things.");


		abandonedSection.Chest.Put("mousetail", mousetail);
		storageRoom.Chest.Put("poopotion", poopotion);
		utilityRoom.Chest.Put("slingshot", slingshot);
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{

			stopwatch.Start();

			Command command = parser.GetCommand();
			OverFlowChamber(command);


			finished = ProcessCommand(command);
			//! if player is NOT alive (!) then finished is true
			if (!player.IsAlive())
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
		Console.WriteLine("You fell into the sewers while working your regular 9-5.");
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
			case "overFlowChamber":
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
		// Item item = player.backpack.Get(itemName);


		if (player.Use(itemName) == false)
		{
			Console.WriteLine($"You don't have a {itemName} to use.");
		}
	}

	private void PrintLook()
	{
		Console.WriteLine("Items in the room: " + player.CurrentRoom.Chest.ShowInventory());
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

		player.Damage(0);

		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());

	}

	//methods
	private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Take what?");
			return;
		}

		string itemName = command.SecondWord;

		Item item = player.CurrentRoom.Chest.Get(itemName);

		if (item == null)
		{
			Console.WriteLine("There is no " + itemName + " in this room.");
			return;
		}

		switch (itemName)
		{
			case "mousetail":
				Console.WriteLine("Why would you even want to pick up a mousetail? You still picked it up tho. Dirty faggot 🤢");
				break;
			case "poopotion":
				Console.WriteLine("You picked up a bottle which looks like all the colors combined... You are wondering if you should drink it.");
				break;
			default:
				Console.WriteLine("You picked up the " + itemName + ".");
				break;
		}

		player.Backpack.Put(itemName, item);

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
}