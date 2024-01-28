namespace Press;

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
