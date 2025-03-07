class Inventory
{
    // Fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    // Constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        items = new Dictionary<string, Item>();
    }

    // Methods
    public int TotalWeight()
    {
        int total = 0;
        foreach (var item in items.Values)
        {
            total += item.Weight;
        }
        return total;
    }

    public int FreeWeight()
    {
        return maxWeight - TotalWeight();
    }

    public bool Put(string itemName, Item item)
    {
        if (item.Weight <= FreeWeight())
        {
            items[itemName] = item;
            return true;
        }
        return false;
    }

    public Item Get(string itemName)
    {
        items.TryGetValue(itemName, out Item item);
        items.Remove(itemName);
        return item;
    }

    public void Remove(string itemName)
    {
        items.Remove(itemName);
    }

    public string ShowInventory()
    {
        if (items.Count == 0)
        {
            return "Nothing";
        }
        return string.Join(", ", items.Keys);
    }
}