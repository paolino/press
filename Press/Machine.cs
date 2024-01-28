
namespace Press;

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
        foreach (RecipeItem item in recipe.Items)
            switch (Insert(item))
            {
                case Problems.NotEnoughTools:
                    return Problems.NotEnoughTools;
                case Problems.OverlappingTools:
                    return Problems.OverlappingTools;
            }
        return Problems.NoProblem;
    }

    public Problems Insert(RecipeItem item)
    {
        switch (Storage.Place(item.Name, item.Position, out Placed? tool))
        {
            case Problems.NotEnoughTools:
                return Problems.NotEnoughTools;
        }
        return Press.Insert(tool!);
    }
}
