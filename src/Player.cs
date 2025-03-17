class Player
{

	//fields
	//auto property
	public int Health { get; private set; }
	public Room CurrentRoom { get; set; }
	public Inventory Backpack { get; private set; }
	//constructor
	public Player()
	{
		CurrentRoom = null;
		Health = 100;

		//100kg because we are strong
		Backpack = new Inventory(25);

	}

	//methods


	//use the item
	public bool Use(string itemName)
	{
		Item item = Backpack.Get(itemName);

		if (item == null)
		{
			return false;
		}

		switch (itemName)
		{
			case "mousetail":
				// CurrentRoom.Chest.Put(itemName, item);
				Console.WriteLine("You used the mousetail. It's a bit disgusting, but you feel a bit better.");
				this.Heal(1);
				Backpack.Remove(itemName);
				break;
			case "poopotion":
				Console.WriteLine("'Ugh, this tastes like absolute shit. Oh wait...' You feel a little worse after drinking this");
				this.Damage(5);
				break;
			case "slingshot":
			Console.WriteLine("U used the slingshot");
			this.Damage(1);		
				break;
				// default:
				// return item.Use(); // Call the Use method on the Item instance


				
		}
		return true;
	}
	public bool TakeFromChest(string itemName)
	{
		// Remove the Item from the Room.
		Item item = CurrentRoom.Chest.Get(itemName);

		if (item == null)
		{
			//!This writeline is not needed! Its only here for conveinience
			Console.WriteLine("There is no " + itemName + " in this room.");
			return false;
		}

		// Check if the item fits in the Backpack
		if (item.Weight > Backpack.FreeWeight())
		{
			//!This writeline is not needed! Its only here for conveinience
			Console.WriteLine("You cannot carry the " + itemName + " Because it's too heavy.");
			// Put the item back in the chest
			CurrentRoom.Chest.Put(itemName, item);
			return false;
		}

		// Put it in your Backpack
		if (Backpack.Put(itemName, item))
		{
			//!This writeline is not needed! Its only here for conveinience
			Console.WriteLine("You have picked up the " + itemName);
			return true;
		}
		return false;
	}


	// public bool DropToChest(string itemName)
	// {
	// 	return false;
	// }

	public int Damage(int amount)
	{
		this.Health -= amount;
		if (this.Health < 0)
		{
			this.Health = 0;
		}
		return this.Health;
	} // player loses some Health

	public int Heal(int amount)
	{

		this.Health += amount;
		if (this.Health > 100)
		{
			this.Health = 100;
		}
		return this.Health;


	} // player gains some Health

	public bool IsAlive()
	{
		if (this.Health == 0)
		{
			// Console.WriteLine("You died, noob! Write 'quit' to exit the game");	
			return false;
		}
		return true;
	} // returns true if player is alive
	
}

