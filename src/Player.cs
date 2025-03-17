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
		Item waterpistol = new Item(1, "waterpistol");
		Backpack.Put("waterpistol", waterpistol);


	}

	//methods


	//use the item
	public bool Use(string itemName, Enemy enemy)
	{
		Item item = Backpack.Get(itemName);

		if (item == null)
		{
			Console.WriteLine($"You don't have a {itemName} to use.");
			return false;
		}

		switch (itemName)
		{
			case "mousetail":
				Console.WriteLine("You used the mousetail. It's a bit disgusting, but you feel a bit better.");
				this.Heal(1);
				Backpack.Remove(itemName);
				break;

			case "poopotion":
				Console.WriteLine("'Ugh, this tastes like absolute shit. Oh wait...' You feel a little worse after drinking this.");
				this.Damage(5);
				break;

			case "slingshot":
				if (enemy != null && enemy.CurrentRoom == this.CurrentRoom)
				{
					Console.WriteLine("You used the slingshot on the enemy!");
					enemy.Damage(10); // Apply 10 damage to the enemy
					if (!enemy.IsAlive())
					{
						Console.WriteLine("You defeated the enemy!");
					}
				}
				else
				{
					Console.WriteLine("There is no enemy here to use the slingshot on.");
				}
				break;

			case "waterpistol":
				Console.WriteLine("You used the water pistol.");
				this.Damage(1);
				break;

			default:
				Console.WriteLine($"You can't use the {itemName}.");
				break;
		}

		return true;
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
