class Enemy
{

    //fields
    //auto property
    public int Health { get; private set; }
    public Room CurrentRoom { get; set; }
    public Inventory Backpack { get; private set; }

    //constructor
    public Enemy()
    {
        CurrentRoom = null;
        Health = 30;

        //25kg because we are strong
        Backpack = new Inventory(25);

    }

    public bool IsAlive()
    {
        if (Health <= 0)
        {
            // Console.WriteLine("The enemy has been defeated!");
            if (CurrentRoom != null)
            {
                CurrentRoom.RemoveEnemy(); 
            }
            return false; 
        }
        return true; 
    }

    public int Damage(int amount)
    {
        this.Health -= amount;
        if (this.Health < 0)
        {
            this.Health = 0;
        }
        return this.Health;
    }
}