public class Item
{
    //fields
    public int Weight { get; }
    public string Desciption { get; }

    //constructor
    public Item (int weight, string description)
    {
        Weight = weight;
        Desciption = description;
    }
}