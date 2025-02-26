class Player
{

	
	//fields
	 public int health { get; set; }
	//auto property
	public Room CurrentRoom { get; set; }
	//constructor
	public Player()
	{
		CurrentRoom = null;
		health = 100;

	}

	//methods
	public int Damage(int amount)
	 { 
		return this.health -= amount;
	 } // player loses some health

	 public int Heal (int amount)
	 { 
		return this.health += amount;
	 } // player gains some health

	 public bool IsAlive()
	 { 
		return health > 0;
	 } // returns true if player is alive
}

