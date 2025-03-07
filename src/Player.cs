class Player
{
	//field



	//fields
	public int health { get; set; }
	//auto property
	public Room CurrentRoom { get; set; }
	public Inventory backpack;
	//constructor
	public Player()
	{
		CurrentRoom = null;
		health = 100;

		//100kg because we are strong
		backpack = new Inventory(100);

	}

	//methods

	public bool TakeFromChest(string itemName)
	{
		// TODO implement:
		// Remove the Item from the Room.
		// Put it in your backpack.
		// Inspect returned values.
		// If the item doesn't fit your backpack, put it back in the chest.
		// Communicate to the user what's happening.
		// Return true/false for success/failure.




		return false;

	}


	public bool DropToChest(string itemName)
	{
		// TODO implement:
		// Remove Item from your inventory.
		// Add the Item to the Room.
		// Inspect returned values.
		// Communicate to the user what's happeni.ng
		// Return true/false for success/failure.

		return false;
	}
	public int Damage(int amount)
	{
		this.health -= amount;
		if (this.health < 0)
		{
			this.health = 0;
		}
		return this.health;
	} // player loses some health

	public int Heal(int amount)
	{

		this.health += amount;
		if (this.health > 100)
		{
			this.health = 100;
		}
		return this.health;


	} // player gains some health

	public bool IsAlive()
	{
		if (this.health == 0)
		{
			// Console.WriteLine("You died, noob! Write 'quit' to exit the game");	
			return false;
		}
		return true;
	} // returns true if player is alive
}

