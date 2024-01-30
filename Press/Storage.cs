namespace Press;

/// <summary>
///
/// A toolStorage is a tool and a quantity
/// </summary>
public class ToolStorage(Tool tool, int quantity) : Tool(tool.Name, tool.Width)
{
    public int Quantity { get; } = quantity;

    /// <summary>
    /// Update the storage quantity of a tool
    /// </summary>
    public ToolStorage Update(int quantity)
    {
        return new ToolStorage(this, Quantity + quantity);
    }
    public override string ToString()
    {
        return $"{Name}->{Quantity}";
    }
    public override bool Equals(object? obj)
    {
        return obj is ToolStorage storage &&
               Name == storage.Name &&
               Width == storage.Width &&
               Quantity == storage.Quantity;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Width, Quantity);
    }
}



/// <summary>
/// A storage is a dictionary of tools and their quantity
///
///
/// </summary>
public class Storage
{
    /// <summary>
    /// The storage content
    /// </summary>
    public Dictionary<String, ToolStorage> Tools { get; } = [];


    /// <summary>
    /// Add a tool quantity to the storage
    /// TODO: check that if we know the tool, it has the same width
    /// </summary>
    /// <param name="key">
    /// The tool to add
    /// </param>
    /// <param name="value">
    /// The quantity to add for the tool
    /// </param>
    public void Store(ToolStorage toolStorage)
    {
        String name = toolStorage.Name;
        if (Tools.TryGetValue(name, out ToolStorage? old))
            Tools[name] = old.Update(toolStorage.Quantity);
        else
            Tools[name] = toolStorage;
    }
    public List<ToolStorage> ToolStorages()
    {
        return Tools.Values.ToList();
    }

    public override String? ToString()
    {
        String output = "Storage:[";
        foreach (ToolStorage toolStorage in Tools.Values)
            output += toolStorage.ToString() + ", ";
        output += "]";
        return output;
    }



    /// <summary>
    /// Pick a tool from the storage return a placed tool
    /// </summary>
    /// <param name="key">
    /// The tool to pick
    /// </param>
    /// <param name="value">
    /// The position where to place the tool
    /// </param>
    /// <param name="placed">
    /// The tool that was placed
    /// </param>
    /// <returns>
    /// true if the tool was placed
    /// false if the tool could not be placed
    /// </returns>
    public Placed Place(String key, int position)
    {
        if (Tools.TryGetValue(key, out ToolStorage? old))
        {
            int quantity = old.Quantity;
            if (quantity < 1) throw new NotEnoughTools();
            ToolStorage newToolStorage = old.Update(-1);
            Tools[key] = newToolStorage;
            return new Placed(newToolStorage, position);
        }
        throw new NotEnoughTools();
    }

}
