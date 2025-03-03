class Inventory
{
    //fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    public int TotalWeight()
    {
        int total = 0;

        //TODO implement
        //Loop through the items, and add all the weights

        return total;
    }

    public int FreeWeight()
    {
        // Return the difference between maxWeight and TotalWeight
        return maxWeight - TotalWeight();
    }
    //constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        items = new Dictionary<string, Item>();
    }

    // methods
    public bool Put(string itemName, Item item)
    {
        //TODO implement:
        //Check the Weight of the Item and check if the Inventory has enough space
        //Does the Item fit?
        //Put Item in the items Dictionary
        //Return true/false for succes/failure

        return false;
    }

    public Item Get(string itemName)
    {
        //TODO implement:
        //Find Item in Items dictionary
        //Remove the Item from the items Dictionary
        //Return the Item or null

        return null;
    }
}

