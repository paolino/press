
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

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
    ///


    public Problems Load(Recipe recipe)
    {
        void Place(Recipe recipe)
        {
            foreach (RecipeItem item in recipe.Items)
                Load(item);
        }
        return Catch(() => Place(recipe));
    }

    private void Load(RecipeItem item)
    {
            Placed placed = Storage.Place(item.Name, item.Position);
            Press.Insert(placed);
    }

    public Problems UnloadByPosition(int position)
    {
        void Unload(int position)
        {
            Placed placed = Press.RemoveByPosition(position);
            Storage.Store(new ToolStorage(placed!, 1));
        }
        return Catch(() => Unload(position));
    }
    /// <summary>
    /// run a press or storage action and catch the exceptions as problems
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    private static Problems Catch(Action  func)
    {
        try
        {
            func();
        }
        catch (OverlappingTools)
        {
            return Problems.OverlappingTools;
        }
        catch (NotEnoughTools)
        {
            return Problems.NotEnoughTools;
        }
        catch (ToolNotFound)
        {
            return Problems.ToolNotFound;
        }
        return Problems.NoProblem;
    }

    public override string ToString()
    {
        return $"Machine:\n   {Storage}\n   {Press}";
    }
}
