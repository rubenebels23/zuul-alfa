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

    // Retrieve the item from the backpack.
    Item item = Backpack.Get(itemName); 

    if (item == null) // If the item is not found, 
    {
      return false; // return false.
    }

    // Handle specific item usage based on the item's name.
    switch (itemName)
		{
			case "mousetail":
				Console.WriteLine("You used the mousetail. It's a bit disgusting, but you feel a bit better.");
				this.Heal(1);
				Backpack.Remove(itemName); // Remove the item after use
				break;

			case "poopotion":
				Console.WriteLine("'Ugh, this tastes like absolute shit. Oh wait...' You feel a little worse after drinking this.");
				this.Damage(5);
				Backpack.Remove(itemName); // Remove the item after use
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


	public bool TakeFromChest(string itemName)
{
    // Retrieve the item from the room's chest
    Item item = CurrentRoom.Chest.Get(itemName);

    // If the item is not found, display a message and return false
    if (item == null)
    {
        Console.WriteLine("There is no " + itemName + " in this room.");
        return false;
    }

    // Check if the item fits in the backpack
    if (item.Weight > Backpack.FreeWeight())
    {
        Console.WriteLine("You cannot carry the " + itemName + " because it's too heavy.");
        // Put the item back in the chest
        CurrentRoom.Chest.Put(itemName, item);
        return false;
    }

    // Add the item to the backpack
    if (Backpack.Put(itemName, item))
    {
        Console.WriteLine("You have picked up the " + itemName + ".");
        return true;
    }

    // If the item could not be added to the backpack, return it to the chest
    CurrentRoom.Chest.Put(itemName, item);
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
		return this.Health > 0;
	} 

}