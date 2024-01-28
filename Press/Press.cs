namespace Press;


public enum Problems
{
    NotEnoughTools,
    OverlappingTools,
    NoProblem
}


/// <summary>
/// A tool is a thing that can has a name and a width
/// </summary>
public class Tool(string name, int width)
{
    public string Name { get; set; } = name;
    public int Width { get; set; } = width;
}

/// <summary>
/// A placed tool is a tool that has a position
/// </summary>
public class Placed(Tool tool, int position) : Tool(tool.Name, tool.Width)
{
    public int Position { get; } = position;
}

/// <summary>
/// A press is a list of placed tools, that are always sorted by position
/// </summary>
public class Press
{
    public List<Placed> Tools { get; } = [];

    private bool CheckPrev(Placed tool, int index)

    {
        if (index == 0) return true; // there is no previous tool
        Placed prev = Tools[index - 1];
        return (prev.Position + prev.Width <= tool.Position);
    }

    /// <summary>
    /// Insert a tool in the recipe
    /// </summary>
    /// <param name="tool"></param>
    /// <returns>
    /// true if the tool was inserted
    /// false if the tool could not be inserted
    ///
    public Problems Insert(Placed tool)
    {
        int index = Tools.FindIndex(y => y.Position > tool.Position);
        if (index < 0) // no placed tool after this one
        {
            if (CheckPrev(tool, Tools.Count)) // respect the previous tool
            {
                Tools.Add(tool);
                return Problems.NoProblem;
            }
        }
        else
        {
            Placed next = Tools[index];
            if (CheckPrev(tool, index)) // respect the next tool
            {
                if (tool.Position + tool.Width <= next.Position)
                {
                    Tools.Insert(index, tool);
                    return Problems.NoProblem;
                }
            }
        }
        return Problems.OverlappingTools;
    }

    /// <summary>
    /// Insert a list of tools
    /// </summary>
    /// <param name="tools"></param>
    /// <returns>
    /// true if all tools were inserted
    /// false as soon as one tool could not be inserted
    /// </returns>
    public Problems InsertTools(List<Placed> tools)
    {
        foreach (Placed tool in tools)
            switch (Insert(tool))
            {
                case Problems.OverlappingTools:
                    return Problems.OverlappingTools;
            }
        return Problems.NoProblem;
    }
}


/// <summary>
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
        String output = "";
        foreach (ToolStorage toolStorage in Tools.Values)
            output += "[" + toolStorage.ToString() + "]";
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
    public Problems Place(String key, int position, out Placed? placed)
    {
        placed = null;
        if (Tools.TryGetValue(key, out ToolStorage? old))
        {
            int quantity = old.Quantity;
            if (quantity < 1) return Problems.NotEnoughTools;
            ToolStorage newToolStorage = old.Update(-1);
            Tools[key] = newToolStorage;
            placed = new Placed(newToolStorage, position);
            return Problems.NoProblem;
        }
        return Problems.NotEnoughTools;
    }

}

/// <summary>
/// A recipeItem is the name of a tool and its position
/// </summary>
public class RecipeItem
{
    public string Name { get; }
    public int Position { get; }

    public RecipeItem(string name, int position)
    {
        Name = name;
        Position = position;
    }
}

/// <summary>
/// A recipe is a list of recipeItems
/// </summary>
public class Recipe(List<RecipeItem> items)
{
    public List<RecipeItem> Items { get; } = items;
}

/// <summary>
/// A machine is a storage and a press
/// </summary>
public class Machine
{

    public Storage Storage { get; } = new Storage();
    public Press Press { get; } = new Press();


    public Machine(List<ToolStorage> tools)
    {
        foreach (ToolStorage tool in tools)
            Storage.Store(tool);
    }
    /// <summary>
    /// Clear the press and put all tools back in the storage
    /// </summary>
    public void Clear()
    {
        foreach (Placed tool in Press.Tools)
            Storage.Store(new ToolStorage(tool, 1));
    }
    /// <summary>
    /// Load a recipe in the press
    /// </summary>
    /// <param name="recipe">
    /// Recipe to load
    /// </param>
    /// <returns>
    /// true if the recipe was loaded
    /// </returns>
    public Problems LoadRecipe(Recipe recipe)
    {
        List<Placed> placed = [];
        foreach (RecipeItem item in recipe.Items)
        {
            switch (Storage.Place(item.Name, item.Position, out Placed? tool))
            {
                case Problems.NotEnoughTools:
                    return Problems.NotEnoughTools;
            }
            placed.Add(tool!);
        }
        return Press.InsertTools(placed);
    }
}
