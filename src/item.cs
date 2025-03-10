public class Item
{
    // Fields
    public int Weight { get; }
    public string Description { get; }

    // Constructor
    public Item(int weight, string description)
    {
        Weight = weight;
        Description = description;
    }

    // Use method
    // public string Use(string itemName)
    // {
    //     // Implement the use logic here
    //     return $"You used the {Description}.";
    // }
}